// 连线canvas

var lineClr = "30,144,255"; // 透明颜色赋值时只能是这种格式
var lineWidth = 4;

var canvas = document.getElementsByClassName("cvs-line");// document.getElementById("bkg-cvs");
var refCell = $("#tb_sum tbody tr:eq(0) td:eq(2)");
var wRate = 0;
var hRate = 0;
updateLine();
//var ctx = canvas.getContext("2d");
//resize();
window.onresize = function () {
    // 延迟开始绘画，如果立即执行有时位置计算会出错
    setTimeout(function () {
        updateLine();
    }, 0);
}
//$(window).resize(function () {
//    // 延迟开始绘画，如果立即执行有时位置计算会出错
//    setTimeout(function () {
//        updateLine();
//    }, 0);
//});

//var RAF = (function () {
//    return window.requestAnimationFrame || window.webkitRequestAnimationFrame || window.mozRequestAnimationFrame || window.oRequestAnimationFrame || window.msRequestAnimationFrame || function (callback) {
//        window.setTimeout(callback, 1000 / 60);
//    };
//})();

function updateLine() {
    wRate = refCell.outerWidth();
    hRate = refCell.outerHeight();

    for (var i = 0; i < canvas.length; i++) {
        //var w = $(canvas[i]).data("w");
        //var h = $(canvas[i]).data("h");
        //console.log('refCell.position().left=' + refCell.position().left);
        //console.log('refCell.position().top=' + refCell.position().top);
        //console.log('wRate=' + wRate);
        //console.log('hRate=' + hRate);

        var w = $(canvas[i]).data("w") * wRate;
        var h = $(canvas[i]).data("h") * hRate;
        canvas[i].width = w > 0 ? w : lineWidth;
        canvas[i].height = h > 0 ? h : lineWidth;

        //var l1 = refCell.offset().left;
        //var l2 = refCell.position().left;
        //var l3 = refCell.css("left");
        // 连线的偏移(相对于表格)
        var offsetX = refCell.position().left + 0.5 * wRate + $(canvas[i]).data("l") * wRate;
        var offsetY = refCell.position().top + 0.5 * hRate + $(canvas[i]).data("t") * hRate;
        $(canvas[i]).css("left", offsetX);
        $(canvas[i]).css("top", offsetY);
    }
    animateLine();
}


// 每一帧循环的逻辑
function animateLine() {
    //var wRate = $("#tb_sum tbody tr:eq(0) td:eq(2)").outerWidth();
    //var hRate = $("#tb_sum tbody tr:eq(0) td:eq(2)").outerHeight();
    for (var i = 0; i < canvas.length; i++) {
        var ctx = canvas[i].getContext("2d");
        ctx.clearRect(0, 0, $(canvas[i]).width(), $(canvas[i]).height());
        var start_x = $(canvas[i]).data("sx") * wRate;
        var start_y = $(canvas[i]).data("sy") * hRate;
        var end_x = $(canvas[i]).data("ex") * wRate;
        var end_y = $(canvas[i]).data("ey") * hRate;

        // 如果是竖线，（让起始位置偏移半个线宽，否则只会画出线宽的一半）
        if (start_x == end_x) {
            start_x += 0.5 * lineWidth;
            end_x += 0.5 * lineWidth;
        }

        // 如果是横线
        if (start_y == end_y) {
            start_y += 0.5 * lineWidth;
            end_y += 0.5 * lineWidth;
        }

        //var dx = Math.abs(end_x - start_x);
        //var dy = Math.abs(end_y - start_y);
        //var dis = Math.sqrt(Math.pow(dx, 2) + Math.pow(dy, 2));
        //var sx = (y - y2) * (x1 - x2) / (y1 - y2) + y2;

        // 画线
        ctx.beginPath();
        ctx.lineWidth = lineWidth;
        ctx.strokeStyle = 'rgba(' + lineClr + ',' + 0.5 + ')';
        ctx.moveTo(start_x, start_y);
        ctx.lineTo(end_x, end_y);
        ctx.stroke();
        ctx.closePath();
    }
    //RAF(animateLine);
}
