'use strict';
console.log('Server running');

const express = require('express');

const app = express();

const net = require('net');

const http = require('http').createServer(app);
const io = require('socket.io')(http);

const messageSize = 6;

var connectedClients = new Set();
var unityServer = false;

var server = net.createServer(function(client) {
    connectedClients.add(client);
    console.log('Client connect. Client local address : ' + client.localAddress + ':' + client.localPort + '. client remote address : ' + client.remoteAddress + ':' + client.remotePort);

    client.setEncoding('utf-8');

    //client.setTimeout(3000);

    // When receive client data.
    client.on('data', function (data) {
        
        // console.log(data);

        while(data.length > 0){
            var messageLength = parseInt(data.slice(0,6));
            data = data.slice(6);
            
            if(messageLength != NaN && messageLength > 0){
                var message = data.slice(0, messageLength);
                data = data.slice(messageLength);
                if(message == "ping"){
                    client.write(encodeMessage("pong"));
                }else if(message == "THIS_IS_UNITY_SERVER"){
                    unityServer = true;
                }else{
                    console.log('Receive client send data : ' + message + ', data size : ' + client.bytesRead);
                    io.emit("message", JSON.stringify({
                        title: "Nachricht vom Spiel:",
                        message: message,
                    }));
                }
    
            }
        }

        // Server send data back to client use client net.Socket object.
        //client.end('Server received data : ' + data + ', send back to client data size : ' + client.bytesWritten);
    });

    // When client send data complete.
    client.on('end', function () {
        console.log('Client disconnect.');
        connectedClients.delete(client);
        // Get current connections count.
        server.getConnections(function (err, count) {
            if(!err)
            {
                // Print current connection count in server console.
                console.log("There are %d connections now. ", count);
                if(unityServer){
                    process.exit();
                }
            }else
            {
                console.error(JSON.stringify(err));
            }

        });
    });

    // When client timeout.
    client.on('timeout', function () {
        console.log('Client request time out. ');
        connectedClients.delete(client);
    })

    client.on('error', function (error) {
        console.error(JSON.stringify(error));
        connectedClients.delete(client);
    });
});

// Make the server a TCP server listening on port 9999.
server.listen(3000, "127.0.0.1", function () {

    // Get server address info.
    var serverInfo = server.address();

    var serverInfoJson = JSON.stringify(serverInfo);

    console.log('TCP server listen on address : ' + serverInfoJson);

    server.on('close', function () {
        console.log('TCP server socket is closed.');
    });

    server.on('error', function (error) {
        console.error(JSON.stringify(error));
    });

});

function sendMessageToClients(message){
    connectedClients.forEach(v => {
        v.write(encodeMessage(message));
    });
}

function encodeMessage(message){

    var l = `${Buffer.byteLength(message, "utf-8")}`;

    for(var i = l.length; i < messageSize; i++){
        l = "0" + l;
    }

    message = l + message;
    
    console.log(`Encoding message: ${message}, length: ${Buffer.byteLength(message, "utf-8")}`);
    return message;
}

// serve files from the public directory
app.use(express.static('public'));

// start the express web server listening on 8080
http.listen(8080, () => {
    console.log('listening on 8080');
});

// serve the homepage
app.get('/', (req, res) => {
    res.sendFile(__dirname + '/index.html');
});

app.post('/clicked/:value', (req, res) => {
    const click = { clickTime: new Date() };
    console.log(req.params.value);
    sendMessageToClients(`Pressed_${req.params.value}`);
    res.send("done");
});

app.post('/qr/:value', (req, res) => {
    const click = { clickTime: new Date() };
    console.log(req.params.value);
    sendMessageToClients(`QR_${req.params.value}`);
    res.send("done");
});

// Socket for receiving message on html page

io.on('connection', () => {
    console.log('User connected to socket.');
});


// -----------------------------------------



var os = require('os');
var ifaces = os.networkInterfaces();

Object.keys(ifaces).forEach(function (ifname) {
    var alias = 0;

    ifaces[ifname].forEach(function (iface) {
        if ('IPv4' !== iface.family || iface.internal !== false) {
            // skip over internal (i.e. 127.0.0.1) and non-ipv4 addresses
            return;
        }

        if (alias >= 1) {
            // this single interface has multiple ipv4 addresses
            console.log(ifname + ':' + alias, iface.address);
        } else {
            // this interface has only one ipv4 adress
            console.log(ifname, iface.address);
        }
        ++alias;
    });
});