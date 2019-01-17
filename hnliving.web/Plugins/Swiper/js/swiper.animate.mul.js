//本插件由www.swiper.com.cn提供,修改成可以执行多个动画(注意连续的两个动画不能同名)
//版本1.03
function swiperAnimateMulCache(a) {
    for (j = 0; j < a.slides.length; j++) {
        for (allBoxes = a.slides[j].querySelectorAll('.ani-mul'), i = 0; i < allBoxes.length; i++) {
            allBoxes[i].attributes['style'] ? allBoxes[i].setAttribute('swiper-animate-style-cache', allBoxes[i].attributes['style'].value) : allBoxes[i].setAttribute('swiper-animate-style-cache', ''),
            allBoxes[i].style.visibility = 'hidden'
        }
    }
}
function swiperAnimateMul(a) {
    clearSwiperAnimateMul(a);
    var b = a.slides[a.activeIndex].querySelectorAll('.ani-mul');
    for (i = 0; i < b.length; i++) {
        b[i].style.visibility = 'visible',
        effect = b[i].attributes['swiper-animate-effect-1'] ? b[i].attributes['swiper-animate-effect-1'].value : '',
        b[i].className = b[i].className + ' ' + effect + ' ' + 'animated',
        style = b[i].attributes['style'].value,
        duration = b[i].attributes['swiper-animate-duration-1'] ? b[i].attributes['swiper-animate-duration-1'].value : '',
        duration && (style = style + 'animation-duration:' + duration + ';-webkit-animation-duration:' + duration + ';'),
        delay = b[i].attributes['swiper-animate-delay-1'] ? b[i].attributes['swiper-animate-delay-1'].value : '',
        delay && (style = style + 'animation-delay:' + delay + ';-webkit-animation-delay:' + delay + ';'),
        iteration = b[i].attributes['swiper-animate-iteration-count-1'] ? b[i].attributes['swiper-animate-iteration-count-1'].value : '',
        iteration && (style = style + 'animation-iteration-count:' + iteration + ';-webkit-animation-iteration-count:' + iteration + ';'),
        b[i].setAttribute('style', style)
    }
}
function clearSwiperAnimateMul(a) {
    for (j = 0; j < a.slides.length; j++) {
        for (allBoxes = a.slides[j].querySelectorAll('.ani-mul'), i = 0; i < allBoxes.length; i++) {
            count = Number(allBoxes[i].attributes['swiper-ani-cnt'].value);
            for (index = 0; index < count; index++) {
                // 去除原先的动画效果
                allBoxes[i].attributes['swiper-animate-effect-' + (index + 1)] && (effect = ' ' + allBoxes[i].attributes['swiper-animate-effect-' + (index + 1)].value, allBoxes[i].className = allBoxes[i].className.replace(effect, ''));

                // 去除原先的动画属性
                style = allBoxes[i].attributes['style'].value,
                duration = allBoxes[i].attributes['swiper-animate-duration-' + (index + 1)].value;
                delay = allBoxes[i].attributes['swiper-animate-delay-' + (index + 1)] ? allBoxes[i].attributes['swiper-animate-delay-' + (index + 1)].value : '';
                iteration = allBoxes[i].attributes['swiper-animate-iteration-count-' + (index + 1)] ? allBoxes[i].attributes['swiper-animate-iteration-count-' + (index + 1)].value : '';
                style = style.replace('animation-duration:' + duration + ';-webkit-animation-duration:' + duration + ';', '');
                delay && (style = style.replace('animation-delay:' + delay + ';-webkit-animation-delay:' + delay + ';', ''));
                iteration && (style = style.replace('animation-iteration-count:' + iteration + ';-webkit-animation-iteration-count:' + iteration + ';', ''));
                allBoxes[i].setAttribute('style', style);
            };
            allBoxes[i].attributes['swiper-animate-style-cache'] && allBoxes[i].setAttribute('style', allBoxes[i].attributes['swiper-animate-style-cache'].value),
            allBoxes[i].style.visibility = 'hidden',
            allBoxes[i].className = allBoxes[i].className.replace(' animated', ''),
            allBoxes[i].attributes['swiper-ani-index'] = '0';
        }
    }
}

bindEvent('.ani-mul');

// 绑定监听
function bindEvent(selector) {
    var x = document.querySelectorAll(selector);

    for (i = 0; i < x.length; i++) {
        // Chrome, Safari 和 Opera 代码
        x[i].addEventListener("webkitAnimationEnd", listenerAniMul);  //动画结束时事件 

        // 标准语法
        x[i].addEventListener("animationend", listenerAniMul);    //动画结束时事件
    }
}

// 
function listenerAniMul() {
    // 总量和序号
    var count = Number(this.attributes['swiper-ani-cnt'].value);
    var index = this.attributes['swiper-ani-index'] ? Number(this.attributes['swiper-ani-index']) : 0;

    // 记录之前动画的号码
    var ind = index + 1;

    // 设置下一个动画的序号
    index++;
    if (index >= count) {
        index = this.attributes['swiper-ani-repeart-ind'] ? Number(this.attributes['swiper-ani-repeart-ind'].value) : 0;;
        if(!this.attributes['swiper-ani-repeat'] || this.attributes['swiper-ani-repeat'].value != '1') {
            return;
        }
    }

    // 去除原先的动画效果
    var eff = ' ' + this.attributes['swiper-animate-effect-' + ind].value + ' animated';
    this.className = this.className.replace(eff, '');

    // 去除原先的动画属性
    style = this.attributes['style'].value,
    duration = this.attributes['swiper-animate-duration-' + ind].value;
    delay = this.attributes['swiper-animate-delay-' + ind] ? this.attributes['swiper-animate-delay-' + ind].value : '';
    iteration = this.attributes['swiper-animate-iteration-count-' + ind] ? this.attributes['swiper-animate-iteration-count-' + ind].value : '';
    style = style.replace('animation-duration:' + duration + ';-webkit-animation-duration:' + duration + ';', '');
    delay && (style = style.replace('animation-delay:' + delay + ';-webkit-animation-delay:' + delay + ';', ''));
    iteration && (style = style.replace('animation-iteration-count:' + iteration + ';-webkit-animation-iteration-count:' + iteration + ';', ''));
    this.setAttribute('style', style);

    // 设置本次动画序号
    this.attributes['swiper-ani-index'] = index;

    // 序号后缀
    var suffix  = '-' + (index + 1);

    effect = this.attributes['swiper-animate-effect' + suffix ] ? this.attributes['swiper-animate-effect' + suffix ].value : '',
    this.className = this.className + ' ' + effect + ' ' + 'animated',
    style = this.attributes['style'].value,
    duration = this.attributes['swiper-animate-duration' + suffix ] ? this.attributes['swiper-animate-duration' + suffix ].value : '',
    duration && (style = style + 'animation-duration:' + duration + ';-webkit-animation-duration:' + duration + ';'),
    delay = this.attributes['swiper-animate-delay' + suffix ] ? this.attributes['swiper-animate-delay' + suffix ].value : '',
    delay && (style = style + 'animation-delay:' + delay + ';-webkit-animation-delay:' + delay + ';'),
    iteration = this.attributes['swiper-animate-iteration-count' + suffix] ? this.attributes['swiper-animate-iteration-count' + suffix].value : '',
    iteration && (style = style + 'animation-iteration-count:' + iteration + ';-webkit-animation-iteration-count:' + iteration + ';'),
    this.setAttribute('style', style)
}