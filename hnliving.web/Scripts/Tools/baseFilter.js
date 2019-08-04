$.fn.extend({
	//灰度
	grayImg : function(data){
		data ? data : 0;
		return $(this).css({
			"filter":"grayscale("+data+"%)",
			 "-webkit-filter": "grayscale("+data+"%)"
		});
	},
	/*模糊*/
	dim : function(data){
		data ? data : 0;
		return $(this).css({
			"filter":"blur("+data+"px)",
			 "-webkit-filter":"blur("+data+"px)"
		});
	},
	//曝光
	brightness : function(data){
		data ? data : 0;
		return $(this).css({
			"filter":"brightness("+data+"%)",
			 "-webkit-filter":"brightness("+data+"%)"
		});
	},
	//对比度
	contrast : function(data){
		data ? data : 0;
		return $(this).css({
			"filter":"contrast("+data+"%)",
			 "-webkit-filter":"contrast("+data+"%)"
		});
	},
	//色相旋转
	hueRotate : function(data){
		data ? data : 0;
		return $(this).css({
			"filter":"hue-rotate("+data+"deg)",
			 "-webkit-filter":"hue-rotate("+data+"deg)"
		});
	},
	//饱和度
	saturate : function(data){
		data ? data : 0;
		return $(this).css({
			"filter":"saturate("+data+"%)",
			 "-webkit-filter":"saturate("+data+"%)"
		});
	},
    //透明度
	opacity: function (data) {
		data ? data : 0;
		return $(this).css({
		    "filter": "opacity("+data+"%)",
		    "-webkit-filter":"opacity("+data+"%)"
		});
	}
})