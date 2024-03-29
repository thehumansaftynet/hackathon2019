// const button = document.getElementById('button_type1');
// button.addEventListener('click', function (e) {
//   console.log('button was clicked');

//   fetch('/clicked', { method: 'POST' })
//     .then(function (response) {
//       if (response.ok) {
//         console.log('Click was recorded');
//         return;
//       }
//       throw new Error('Request failed.');
//     })
//     .catch(function (error) {
//       console.log(error);
//     });
// });



function openQRCamera(node) {
  var val = "";
  var reader = new FileReader();
  reader.onload = function() {
    val = "";
    qrcode.callback = function(res) {
      if(res instanceof Error) {
        alert("No QR code found. Please make sure the QR code is within the camera's frame and try again.");
      } else {
        val = res;
        
        fetch(`/qr/${val}`, {
          method: 'POST',
          body: JSON.stringify({value: val}),
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
          },
        })
          .then(function (response) {
            if (response.ok) {
              console.log('Click was recorded');
              return;
            }
            throw new Error('Request failed.');
          })
          .catch(function (error) {
            console.log(error);
        });

      }
    };
    qrcode.decode(reader.result);
  };
  reader.readAsDataURL(node.files[0]);
}

$('.numberbutton').on('click', function () {
  console.log('button was clicked');

  var value = $(this).attr("value");

  fetch(`/clicked/${value}`, {
    method: 'POST',
    body: JSON.stringify({value: $(this).attr("value")}),
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
  })
    .then(function (response) {
      if (response.ok) {
        console.log('Click was recorded');
        return;
      }
      throw new Error('Request failed.');
    })
    .catch(function (error) {
      console.log(error);
    });
});

var socket = io();
const notificationMessage = window.createNotification({
  closeOnClick: false,
  displayCloseButton: true,

  // nfc-top-left
  // nfc-bottom-right
  // nfc-bottom-left
  positionClass: 'nfc-bottom-right',

  // callback
  onclick: false,

  // timeout in milliseconds
  showDuration: 10000,

  // success, info, warning, error, and none
  theme: 'info'
});

socket.on("message", (s) => {
  var data = JSON.parse(s);
  if(data.message && data.message.substring(0,3) == "CT_"){
    ChangeButtonText(data.message.substring(5,data.message.length), data.message.substring(3,4));
  }else if(data.message && data.message.substring(0,3) == "BP_"){
    SetProposedButton(data.message.substring(3,4));
  }else{
    notificationMessage({
      title: data.title,
      message: data.message,
    });
  }
  
});

function ChangeButtonText(text, buttonID){
  $("#button_id_" + buttonID.toString()).text(text);
}

function SetProposedButton(buttonID){
  for (i = 0; i <= 5; i++) {
    $("#button_id_" + i.toString()).css("borderColor", "blue");
  }
  if(buttonID >= 0){
    $("#button_id_" + buttonID.toString()).css("borderColor", "lime");
  }
}