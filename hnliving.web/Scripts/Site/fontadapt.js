var rem = 18;       // 1个rem是多少像素
var rate = 2;       // 宽高比例
fontAdapt();

// 字体适配不同屏幕大小
function fontAdapt() {
    rem = document.documentElement.clientWidth / 640 * 10;// * 100
    rate = document.documentElement.clientWidth / document.documentElement.clientHeight;
    if (rem < 12) {
        rem = 12;
    }
    document.documentElement.style.fontSize = rem + 'px';
    //console.log('rate='+rate);
}

$().ready(function () {
    // 窗口改变大小事件
    $(window).resize(function () {
        // 字体适配不同屏幕大小
        fontAdapt();
    })
})