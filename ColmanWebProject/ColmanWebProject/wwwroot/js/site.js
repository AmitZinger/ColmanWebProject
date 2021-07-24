
// Canvas settings
var fontBase = 1000,                   // selected default width for canvas
    fontSize = 70;                     // default size for font

function getFont() {
    var ratio = fontSize / fontBase;   
    var size = c.width * ratio;   // get font size based on current width
    return (size | 0) + 'px Comic Sans MS'; // set font
}

var c = document.getElementById("myCanvas");
var ctx = c.getContext("2d");
ctx.fillStyle = "#191970";
ctx.font = getFont();
ctx.fillText("Colman's Music Online Store!", 10, 50);
