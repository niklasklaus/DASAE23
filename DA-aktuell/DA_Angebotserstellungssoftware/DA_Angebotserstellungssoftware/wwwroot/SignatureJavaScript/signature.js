window.onload = function () {
    var canvas = document.getElementById('canvas');
    var context = canvas.getContext('2d');
    var isDrawing = false;
    // Draw a dashed line to indicate where not to sign
    context.beginPath();
    context.moveTo(10, 50); // Starting point of the line
    context.lineTo(390, 50); // Ending point of the line
    context.lineWidth = 1; // Set line width
    context.strokeStyle = 'black'; // Set line color
    context.stroke(); // Draw the line

    // Funktionen für Touch-Ereignisse
    canvas.addEventListener('touchstart', function (e) {
        isDrawing = true;
        context.beginPath();
        var touch = e.touches[0];
        context.moveTo(touch.clientX - canvas.offsetLeft, touch.clientY - canvas.offsetTop);
    });

    canvas.addEventListener('touchmove', function (e) {
        if (isDrawing) {
            var touch = e.touches[0];
            context.lineTo(touch.clientX - canvas.offsetLeft, touch.clientY - canvas.offsetTop);
            context.stroke();
        }
    });

    canvas.addEventListener('touchend', function () {
        isDrawing = false;
    });

    // Funktionen für Mausereignisse (optional, für Desktop-Kompatibilität)
    canvas.addEventListener('mousedown', function (e) {
        isDrawing = true;
        context.beginPath();
        context.moveTo(e.clientX - canvas.offsetLeft, e.clientY - canvas.offsetTop);
    });

    canvas.addEventListener('mousemove', function (e) {
        if (isDrawing) {
            context.lineTo(e.clientX - canvas.offsetLeft, e.clientY - canvas.offsetTop);
            context.stroke();
        }
    });

    canvas.addEventListener('mouseup', function () {
        isDrawing = false;
    });

    document.querySelector('.canvas-clear').addEventListener('click', function () {
        context.clearRect(0, 0, canvas.width, canvas.height);
        context.beginPath();
        context.moveTo(10, 50); // Starting point of the line
        context.lineTo(390, 50); // Ending point of the line
        context.lineWidth = 1; // Set line width
        context.strokeStyle = 'black'; // Set line color
        context.stroke(); // Draw the line
    });
};

function getSignatureValue() {
    var canvas = document.getElementById('canvas');
    var context = canvas.getContext('2d');
    // Clear the area of the dashed line before getting the signature
    context.clearRect(0, 48, canvas.width, 3);
    var signatureImage = canvas.toDataURL('image/png');
    // Clear canvas after capturing the signature
    context.clearRect(0, 0, canvas.width, canvas.height);
    // Redraw the dashed line
    context.beginPath();
    context.moveTo(10, 50); // Starting point of the line
    context.lineTo(390, 50); // Ending point of the line
    context.lineWidth = 1; // Set line width
    context.strokeStyle = 'black'; // Set line color
    context.stroke(); // Draw the line
    return signatureImage;
}
