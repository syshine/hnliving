!function (t, i) {
    "object" == typeof exports && "object" == typeof module ? module.exports = i() : "function" == typeof define && define.amd ? define("ClChart", [], i) : "object" == typeof exports ? exports.ClChart = i() : t.ClChart = i()
}(window, function () {
    return function (t) {
        var i = {};
        function e(n) {
            if (i[n])
                return i[n].exports;
            var a = i[n] = {
                i: n,
                l: !1,
                exports: {}
            };
            return t[n].call(a.exports, a, a.exports, e),
            a.l = !0,
            a.exports
        }
        return e.m = t,
        e.c = i,
        e.d = function (t, i, n) {
            e.o(t, i) || Object.defineProperty(t, i, {
                enumerable: !0,
                get: n
            })
        }
        ,
        e.r = function (t) {
            "undefined" != typeof Symbol && Symbol.toStringTag && Object.defineProperty(t, Symbol.toStringTag, {
                value: "Module"
            }),
            Object.defineProperty(t, "__esModule", {
                value: !0
            })
        }
        ,
        e.t = function (t, i) {
            if (1 & i && (t = e(t)),
            8 & i)
                return t;
            if (4 & i && "object" == typeof t && t && t.__esModule)
                return t;
            var n = Object.create(null);
            if (e.r(n),
            Object.defineProperty(n, "default", {
                enumerable: !0,
                value: t
            }),
            2 & i && "string" != typeof t)
                for (var a in t)
                    e.d(n, a, function (i) {
                        return t[i]
                    }
                    .bind(null, a));
            return n
        }
        ,
        e.n = function (t) {
            var i = t && t.__esModule ? function () {
                return t.default
            }
            : function () {
                return t
            }
            ;
            return e.d(i, "a", i),
            i
        }
        ,
        e.o = function (t, i) {
            return Object.prototype.hasOwnProperty.call(t, i)
        }
        ,
        e.p = "",
        e(e.s = 4)
    }([function (t, i, e) {
        "use strict";
        function n(t, i) {
            return ("000000000" + t).slice(-1 * (i = 9 < i ? 9 : i))
        }
        function a(t) {
            var i;
            return void 0 === t ? new Date : (i = "string" == typeof t ? parseInt(t) : t,
            isNaN(i) ? new Date : new Date(1e3 * i))
        }
        function s(t) {
            var i = a(t);
            return 1e4 * i.getFullYear() + 100 * (i.getMonth() + 1) + i.getDate()
        }
        function o(t) {
            var i = a(t);
            return 100 * i.getHours() + i.getMinutes()
        }
        function h(t) {
            return new Date(Math.floor(t / 1e4), Math.floor(t % 1e4 / 100) - 1, t % 100).getDay()
        }
        function r(t) {
            return new Date(Math.floor(t / 1e4), Math.floor(t % 1e4 / 100) - 1, t % 100).getMonth() + 1
        }
        function l(t) {
            return new Date(Math.floor(t / 1e4), Math.floor(t % 1e4 / 100) - 1, t % 100) / 1e3
        }
        function c(t, i) {
            return Math.floor((l(i) - l(t)) / 86400)
        }
        function f(t, i, e) {
            var s = a(t);
            switch (i) {
                case "minute":
                    return void 0 === e ? s.getHours() + ":" + n(s.getMinutes(), 2) : o(t) === o(e) ? ":" + n(s.getSeconds(), 2) : s.getHours() + ":" + n(s.getMinutes(), 2);
                case "datetime":
                    return 1e4 * s.getFullYear() + 100 * (s.getMonth() + 1) + s.getDate() + "-" + s.getHours() + ":" + n(s.getMinutes(), 2);
                default:
                    return ""
            }
        }
        function u(t, i) {
            return 60 * (Math.floor(i / 100) - Math.floor(t / 100)) + i % 100 - t % 100
        }
        function d(t, i) {
            var e = 60 * Math.floor(t / 100) + t % 100 + i;
            return 100 * Math.floor(e / 60) + e % 60
        }
        function v(t) {
            var i;
            if (Array.isArray(t)) {
                i = [];
                for (var e = t.length, n = 0; n < e; n++)
                    i[n] = v(t[n])
            } else
                i = t;
            return i
        }
        function m(t) {
            var i;
            if (t instanceof Object)
                if (Array.isArray(t))
                    i = v(t);
                else
                    for (var e in i = {},
                    t)
                        i[e] = m(t[e]);
            else
                i = t;
            return i
        }
        function x(t, i) {
            var e;
            if (i instanceof Object)
                if (Array.isArray(i))
                    for (var n in e = [],
                    i)
                        e[n] = t && t[n] ? x(t[n], i[n]) : v(i[n]);
                else
                    for (var a in e = {},
                    i)
                        e[a] = t && t[a] ? x(t[a], i[a]) : m(i[a]);
            else
                e = t || i;
            return e
        }
        function y(t) {
            return !(void 0 !== t && Array.isArray(t) && 0 < t.length)
        }
        function g(t, i) {
            return void 0 === t ? {
                left: 0,
                top: 0,
                width: 0,
                height: 0
            } : void 0 === i ? t : {
                left: t.left + i.left,
                top: t.top + i.top,
                width: t.width - (i.left + i.right),
                height: t.height - (i.top + i.bottom)
            }
        }
        function p(t, i) {
            return void 0 !== t && void 0 !== i && i.x >= t.left && i.y >= t.top && i.x < t.left + t.width && i.y < t.top + t.height
        }
        function b(t, i) {
            return void 0 !== t && void 0 !== i && i >= t.left && i < t.left + t.width
        }
        function k(t, i) {
            return void 0 !== t && void 0 !== i && i >= t.top && i < t.top + t.height
        }
        function w(t, i) {
            return !(i.indexOf(t) < 0)
        }
        function I(t, i) {
            for (var e = [], n = 0; n < t.length; n++)
                for (var a = 0; a < i.length; a++)
                    if (t[n] === i[a]) {
                        e.push(t[n]);
                        break
                    }
            return e
        }
        function M(t, i) {
            if (void 0 === t || isNaN(t))
                return "--";
            "string" == typeof t && (t = parseFloat(t)),
            void 0 === i && (i = 100);
            var e = t / i;
            return e = 1e11 < e ? (e / 1e8).toFixed(0) + "亿" : 1e10 < e ? (e / 1e8).toFixed(1) + "亿" : 1e9 < e ? (e / 1e8).toFixed(2) + "亿" : 1e8 < e ? (e / 1e8).toFixed(3) + "亿" : 1e7 < e ? (e / 1e4).toFixed(0) + "万" : 1e6 < e ? (e / 1e4).toFixed(1) + "万" : -1e-9 < e && e < 1e-9 ? "--" : e.toFixed(0),
            String(e)
        }
        function O(t, i, e, n) {
            if (void 0 === t || isNaN(t))
                return "--";
            var a = t;
            return (void 0 === i || i < 0 || 10 < i) && (i = 0),
            -1e-9 < t && t < 1e-9 && !n ? "--" : (a = a.toFixed(i),
            void 0 === e || e < 4 ? a : a.substr(0, e))
        }
        function D(t, i, e) {
            var a, s = i;
            switch (t) {
                case "M5":
                case "M15":
                case "M30":
                case "M60":
                    s = f(i, "datetime");
                    break;
                case "MIN":
                    s = void 0 === e ? f(i, "minute") : (a = e,
                    n(Math.floor(a / 100), 2).toString() + ":" + n(a % 100, 2).toString());
                    break;
                case "DAY5":
                    s = f(i, "minute")
            }
            return s
        }
        function C(t, i, e, n, a) {
            return "rate" === i ? function (t, i) {
                if (void 0 === t || isNaN(t) || void 0 === i)
                    return "--";
                "string" == typeof t && (t = parseFloat(t));
                var e = Math.abs((t - i) / i * 100);
                return e.toFixed(2) + "%"
            }(t, a) : "price" === i ? (void 0 === e && (e = 2),
            O(t, e, 7)) : (void 0 === n && (n = 100),
            M(t, n))
        }
        e.d(i, "h", function () {
            return s
        }),
        e.d(i, "l", function () {
            return o
        }),
        e.d(i, "k", function () {
            return h
        }),
        e.d(i, "j", function () {
            return r
        }),
        e.d(i, "i", function () {
            return c
        }),
        e.d(i, "g", function () {
            return f
        }),
        e.d(i, "m", function () {
            return u
        }),
        e.d(i, "n", function () {
            return d
        }),
        e.d(i, "a", function () {
            return v
        }),
        e.d(i, "b", function () {
            return m
        }),
        e.d(i, "v", function () {
            return x
        }),
        e.d(i, "t", function () {
            return y
        }),
        e.d(i, "u", function () {
            return g
        }),
        e.d(i, "r", function () {
            return p
        }),
        e.d(i, "p", function () {
            return b
        }),
        e.d(i, "q", function () {
            return k
        }),
        e.d(i, "o", function () {
            return w
        }),
        e.d(i, "s", function () {
            return I
        }),
        e.d(i, "f", function () {
            return M
        }),
        e.d(i, "d", function () {
            return O
        }),
        e.d(i, "e", function () {
            return D
        }),
        e.d(i, "c", function () {
            return C
        })
    }
    , function (t, i, e) {
        "use strict";
        e.d(i, "d", function () {
            return s
        }),
        e.d(i, "i", function () {
            return r
        }),
        e.d(i, "n", function () {
            return f
        }),
        e.d(i, "o", function () {
            return u
        }),
        e.d(i, "f", function () {
            return d
        }),
        e.d(i, "e", function () {
            return v
        }),
        e.d(i, "k", function () {
            return m
        }),
        e.d(i, "c", function () {
            return x
        }),
        e.d(i, "b", function () {
            return y
        }),
        e.d(i, "a", function () {
            return g
        }),
        e.d(i, "p", function () {
            return p
        }),
        e.d(i, "m", function () {
            return b
        }),
        e.d(i, "l", function () {
            return k
        }),
        e.d(i, "j", function () {
            return w
        }),
        e.d(i, "h", function () {
            return I
        }),
        e.d(i, "g", function () {
            return M
        });
        var n = e(2)
          , a = e(0);
        function s(t, i) {
            var e = t.fields
              , n = t.value
              , a = 2 < arguments.length && void 0 !== arguments[2] ? arguments[2] : 0
              , o = 0;
            try {
                var h = n;
                switch (Array.isArray(n[0]) && (h = n[a]),
                i) {
                    case "idx":
                        o = void 0 === e.idx ? a : h[e.idx];
                        break;
                    case "coinzoom":
                        o = Math.pow(10, s({
                            fields: e,
                            value: n
                        }, "coinzoom", 0));
                        break;
                    case "volzoom":
                        o = Math.pow(10, s({
                            fields: e,
                            value: n
                        }, "volunit", 0));
                        break;
                    case "flow":
                    case "total":
                        o = 100 * h[e[i]];
                        break;
                    case "decvol":
                        o = 0 === a ? h[e.vol] : h[e.vol] - s({
                            fields: e,
                            value: n
                        }, "vol", a - 1);
                        break;
                    case "decmoney":
                        o = 0 === a ? h[e.money] : h[e.money] - s({
                            fields: e,
                            value: n
                        }, "money", a - 1);
                        break;
                    default:
                        h[e[i]] && (o = h[e[i]])
                }
            } catch (t) {
                o = 0
            }
            return o
        }
        function o(t) {
            var i = 1e3
              , e = 0
              , a = 0;
            return t[n.FIELD_RIGHT.accrual] && (a = t[n.FIELD_RIGHT.accrual] / 10),
            (0 < t[n.FIELD_RIGHT.sendstock] || 0 < t[n.FIELD_RIGHT.allotstock]) && (i = 1e3 + t[n.FIELD_RIGHT.sendstock] / 10 + t[n.FIELD_RIGHT.allotstock] / 10,
            e = -t[n.FIELD_RIGHT.allotprice] * t[n.FIELD_RIGHT.allotstock] / 1e4),
            {
                gs: i,
                pg: e,
                px: a
            }
        }
        function h(t, i, e) {
            return (t = "forword" === e ? 1e3 * (1e3 * t - i.pg - i.px) / i.gs : 1e3 * t * i.gs / 1e3 + i.pg + i.px) / 1e3
        }
        function r(t, i, e, n) {
            if (void 0 === n || n.length < 1)
                return e;
            for (var a = 0; a < n.length; a++)
                if (n[a][0] > t && n[a][0] <= i) {
                    e = h(e, o(n[a]), "forword");
                    break
                }
            return e
        }
        function l(t, i, e, a, s) {
            var r = o(i);
            if ("forword" === e)
                for (var l = a; l < s; l++)
                    t[l][n.FIELD_DAY.open] = h(t[l][n.FIELD_DAY.open], r, e),
                    t[l][n.FIELD_DAY.high] = h(t[l][n.FIELD_DAY.high], r, e),
                    t[l][n.FIELD_DAY.low] = h(t[l][n.FIELD_DAY.low], r, e),
                    t[l][n.FIELD_DAY.close] = h(t[l][n.FIELD_DAY.close], r, e),
                    t[l][n.FIELD_DAY.vol] = t[l][n.FIELD_DAY.vol] * r.gs / 1e3
        }
        function c(t, i, e) {
            return t < e && e <= i
        }
        function f(t, i, e, a, s) {
            if (i.length < 1 || t.length < 1)
                return t;
            if (void 0 === e && (e = "forword"),
            (void 0 === a || a < 0 || a > t.length - 1) && (a = 0),
            (void 0 === s || s < 0) && (s = t.length - 1),
            "forword" === e)
                for (var o = a; o <= s; o++)
                    for (var h = 0; h < i.length; h++)
                        if (!(o < 1) && c(t[o - 1][n.FIELD_DAY.time], t[o][n.FIELD_DAY.time], i[h][n.FIELD_RIGHT.time])) {
                            l(t, i[h], e, a, o);
                            break
                        }
            return t
        }
        function u(t, i, e, s, o) {
            if (i.length < 1 || t.length < 1)
                return t;
            if (void 0 === e && (e = "forword"),
            (void 0 === s || s < 0 || s > t.length - 1) && (s = 0),
            (void 0 === o || o < 0) && (o = t.length - 1),
            "forword" === e)
                for (var h = s; h <= o; h++)
                    for (var r = 0; r < i.length; r++)
                        if (!(h < 1) && c(Object(a.h)(t[h - 1][n.FIELD_DAY.time]), Object(a.h)(t[h][n.FIELD_DAY.time]), i[r][n.FIELD_RIGHT.time])) {
                            l(t, i[r], e, s, h);
                            break
                        }
            return t
        }
        function d(t, i) {
            if (t.value.length < 1)
                return {
                    status: "new",
                    index: -1
                };
            var e = t.value[t.value.length - 1][t.fields.time];
            return e === i ? {
                status: "old",
                index: t.value.length - 1
            } : i < e ? {
                status: "error",
                index: t.value.length - 1
            } : {
                status: "new",
                index: t.value.length - 1
            }
        }
        function v(t, i) {
            return void 0 === t || void 0 === t.value || t.value.length < 1 ? {
                finded: !1,
                index: -1
            } : i === t.value[t.value.length - 1][t.fields.time] ? {
                finded: !0,
                index: t.value.length - 1
            } : {
                finded: !1,
                index: -1
            }
        }
        function m(t) {
            return void 0 === t || Object(a.t)(t.value) ? 0 : t.value.length
        }
        function x(t, i) {
            return !(Array.isArray(t) && 0 < t[i.open] && 0 < t[i.high] && 0 < t[i.low] && 0 < t[i.close] && 0 < t[i.vol] && 0 < t[i.money])
        }
        function y(t) {
            var i = [];
            if (!Array.isArray(t))
                return i;
            for (var e = 0; e < t.length; e++)
                x(t[e], n.FIELD_DAY) || i.push(t[e]);
            return i
        }
        function g(t, i, e) {
            var s = [];
            if (t.length < 1)
                return s;
            var o, h = 5;
            !Object(a.h)(t[t.length - 1][n.FIELD_DAY5.time]) === i && (h = 4);
            var r = 0
              , l = 0;
            for (o = t.length - 1; 0 <= o; o--) {
                if (l !== Object(a.h)(t[o][n.FIELD_DAY5.time]) && (l = Object(a.h)(t[o][n.FIELD_DAY5.time]),
                h < ++r)) {
                    o++;
                    break
                }
                s.splice(0, 0, [t[o][n.FIELD_DAY5.time], t[o][n.FIELD_DAY5.close], t[o][n.FIELD_DAY5.vol]])
            }
            var c = l = r = 0
              , f = 0
              , u = w(e);
            for (o = 0; o < s.length; o++) {
                l !== Object(a.h)(s[o][n.FIELD_DAY5.time]) && (l = Object(a.h)(s[o][n.FIELD_DAY5.time]),
                r++,
                f = c = 0),
                c += s[o][n.FIELD_DAY5.vol],
                f += s[o][n.FIELD_DAY5.close] * s[o][n.FIELD_DAY5.vol];
                var d = I(s[o][n.FIELD_DAY5.time], e);
                d += (r - 1) * u,
                s[o][n.FIELD_DAY5.idx] = d,
                s[o][n.FIELD_DAY5.allvol] = c,
                s[o][n.FIELD_DAY5.allmoney] = f
            }
            return s
        }
        function p(t, i) {
            var e = s({
                fields: t,
                value: i
            }, "coinzoom")
              , n = s({
                  fields: t,
                  value: i
              }, "volzoom");
            return {
                stktype: s({
                    fields: t,
                    value: i
                }, "type"),
                volzoom: n,
                volunit: s({
                    fields: t,
                    value: i
                }, "volunit"),
                coinzoom: e,
                coinunit: s({
                    fields: t,
                    value: i
                }, "coinunit"),
                before: s({
                    fields: t,
                    value: i
                }, "before"),
                stophigh: s({
                    fields: t,
                    value: i
                }, "stophigh"),
                stoplow: s({
                    fields: t,
                    value: i
                }, "stoplow")
            }
        }
        function b(t) {
            for (var i = [], e = [], s = n.FIELD_DAY, o = !0, h = 0; h < t.length; h++) {
                o ? (e[s.open] = t[h][s.open],
                e[s.high] = t[h][s.high],
                e[s.low] = t[h][s.low],
                o = !1) : (e[s.high] = e[s.high] > t[h][s.high] ? e[s.high] : t[h][s.high],
                e[s.low] = e[s.low] < t[h][s.low] || 0 === t[h][s.low] ? e[s.low] : t[h][s.low]),
                e[s.close] = t[h][s.close],
                e[s.vol] = t[h][s.vol],
                e[s.money] = t[h][s.money];
                var r = Object(a.k)(t[h][s.time]);
                (h >= t.length - 1 || 5 === r || 7 < Object(a.i)(t[h][s.time], t[h + 1][s.time]) + r) && (e[s.time] = t[h][s.time],
                i.push(Object(a.a)(e)),
                o = !0)
            }
            return o || (e[s.time] = t[t.length - 1][s.time],
            i.push(Object(a.a)(e))),
            i
        }
        function k(t) {
            for (var i, e = [], s = [], o = n.FIELD_DAY, h = !0, r = 0; r < t.length; r++)
                h ? (s[o.open] = t[r][o.open],
                s[o.high] = t[r][o.high],
                s[o.low] = t[r][o.low],
                i = Object(a.j)(t[r][o.time]),
                h = !1) : (s[o.high] = s[o.high] > t[r][o.high] ? s[o.high] : t[r][o.high],
                s[o.low] = s[o.low] < t[r][o.low] || 0 === t[r][o.low] ? s[o.low] : t[r][o.low]),
                s[o.close] = t[r][o.close],
                s[o.vol] = t[r][o.vol],
                s[o.money] = t[r][o.money],
                (r >= t.length - 1 || Object(a.j)(t[r + 1][o.time]) !== i) && (s[o.time] = t[r][o.time],
                e.push(Object(a.a)(s)),
                h = !0);
            return h || (s[o.time] = t[t.length - 1][o.time],
            e.push(Object(a.a)(s))),
            e
        }
        function w(t) {
            for (var i = 0, e = 0; e < t.length; e++)
                i += Object(a.m)(t[e].begin, t[e].end);
            return i
        }
        function I(t, i) {
            for (var e = Object(a.l)(t), n = 0, s = -1, o = 0; o < i.length; o++) {
                if (e > i[o].begin && e < i[o].end) {
                    s = n + Object(a.m)(i[o].begin, e);
                    break
                }
                if (e <= i[o].begin && 0 === o)
                    return 0;
                if (e <= i[o].begin && e > Object(a.n)(i[o].begin, -5))
                    return n;
                if (n += Object(a.m)(i[o].begin, i[o].end),
                e >= i[o].end && o === i.length - 1)
                    return n - 1;
                if (e >= i[o].end && e < Object(a.n)(i[o].end, 5))
                    return n - 1
            }
            return s
        }
        function M(t, i, e) {
            for (var n = t, s = 0, o = 0, h = 0; h < i.length; h++) {
                if (n < (o = Object(a.m)(i[h].begin, i[h].end))) {
                    s = Object(a.n)(i[h].begin, n + 1);
                    var r = new Date(Math.floor(e / 1e4), Math.floor(e % 1e4 / 100) - 1, e % 100, Math.floor(s / 100), s % 100, 0);
                    return Math.floor(r / 1e3)
                }
                n -= o
            }
            return 0
        }
    }
    , function (t, i, e) {
        "use strict";
        e.r(i),
        e.d(i, "STOCK_TYPE_INDEX", function () {
            return n
        }),
        e.d(i, "STOCK_TYPE_A", function () {
            return a
        }),
        e.d(i, "STOCK_TYPE_B", function () {
            return s
        }),
        e.d(i, "STOCK_TYPE_OTHER", function () {
            return o
        }),
        e.d(i, "STOCK_TRADETIME", function () {
            return h
        }),
        e.d(i, "FIELD_DAY", function () {
            return r
        }),
        e.d(i, "FIELD_MIN", function () {
            return l
        }),
        e.d(i, "FIELD_TICK", function () {
            return c
        }),
        e.d(i, "FIELD_DAY5", function () {
            return f
        }),
        e.d(i, "FIELD_LINE", function () {
            return u
        }),
        e.d(i, "FIELD_ILINE", function () {
            return d
        }),
        e.d(i, "FIELD_NOW", function () {
            return v
        }),
        e.d(i, "FIELD_ENOW", function () {
            return m
        }),
        e.d(i, "FIELD_NOW_IDX", function () {
            return x
        }),
        e.d(i, "FIELD_INFO", function () {
            return y
        }),
        e.d(i, "FIELD_RIGHT", function () {
            return g
        }),
        e.d(i, "FIELD_FINANCE", function () {
            return p
        }),
        e.d(i, "FIELD_TRADE", function () {
            return b
        });
        var n = 0
          , a = 1
          , s = 4
          , o = 5
          , h = [{
              begin: 930,
              end: 1130
          }, {
              begin: 1300,
              end: 1500
          }]
          , r = {
              time: 0,
              open: 1,
              high: 2,
              low: 3,
              close: 4,
              vol: 5,
              money: 6
          }
          , l = {
              idx: 0,
              open: 1,
              high: 2,
              low: 3,
              close: 4,
              vol: 5,
              money: 6,
              allvol: 5,
              allmoney: 6
          }
          , c = {
              time: 0,
              close: 1,
              vol: 2
          }
          , f = {
              time: 0,
              close: 1,
              vol: 2,
              idx: 3,
              allvol: 4,
              allmoney: 5
          }
          , u = {
              time: 0,
              value: 1
          }
          , d = {
              idx: 0,
              value: 1
          }
          , v = {
              time: 0,
              open: 1,
              high: 2,
              low: 3,
              close: 4,
              vol: 5,
              money: 6,
              buy1: 7,
              buyvol1: 8,
              sell1: 9,
              sellvol1: 10,
              buy2: 11,
              buyvol2: 12,
              sell2: 13,
              sellvol2: 14,
              buy3: 15,
              buyvol3: 16,
              sell3: 17,
              sellvol3: 18,
              buy4: 19,
              buyvol4: 20,
              sell4: 21,
              sellvol4: 22,
              buy5: 23,
              buyvol5: 24,
              sell5: 25,
              sellvol5: 26
          }
          , m = {
              time: 0,
              open: 1,
              high: 2,
              low: 3,
              close: 4,
              vol: 5,
              money: 6,
              buy1: 7,
              buyvol1: 8,
              sell1: 9,
              sellvol1: 10
          }
          , x = {
              time: 0,
              open: 1,
              high: 2,
              low: 3,
              close: 4,
              vol: 5,
              money: 6,
              ups: 7,
              upvol: 8,
              downs: 9,
              downvol: 10,
              mids: 11,
              midvol: 12
          }
          , y = {
              marker: 0,
              code: 1,
              name: 2,
              search: 3,
              type: 4,
              coinunit: 5,
              volunit: 6,
              before: 7,
              stophigh: 8,
              stoplow: 9
          }
          , g = {
              time: 0,
              sendstock: 1,
              accrual: 2,
              allotstock: 3,
              allotprice: 4
          }
          , p = {
              code: 0,
              time: 1,
              flow: 2,
              total: 3,
              earnings: 4
          }
          , b = {
              time: 0,
              code: 1,
              type: 2,
              price: 3,
              vol: 4,
              money: 5,
              info: 6
          }
    }
    , function (module, __webpack_exports__, __webpack_require__) {
        "use strict";
        __webpack_require__.d(__webpack_exports__, "a", function () {
            return ClFormula
        });
        var _data_cl_data_tools__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(1);
        function _toConsumableArray(t) {
            return _arrayWithoutHoles(t) || _iterableToArray(t) || _nonIterableSpread()
        }
        function _nonIterableSpread() {
            throw new TypeError("Invalid attempt to spread non-iterable instance")
        }
        function _iterableToArray(t) {
            if (Symbol.iterator in Object(t) || "[object Arguments]" === Object.prototype.toString.call(t))
                return Array.from(t)
        }
        function _arrayWithoutHoles(t) {
            if (Array.isArray(t)) {
                for (var i = 0, e = new Array(t.length) ; i < t.length; i++)
                    e[i] = t[i];
                return e
            }
        }
        function _classCallCheck(t, i) {
            if (!(t instanceof i))
                throw new TypeError("Cannot call a class as a function")
        }
        function _defineProperties(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        function _createClass(t, i, e) {
            return i && _defineProperties(t.prototype, i),
            e && _defineProperties(t, e),
            t
        }
        var ClFormula = function () {
            function ClFormula() {
                _classCallCheck(this, ClFormula),
                this.source = {
                    data: {},
                    minIndex: 0,
                    maxIndex: 0,
                    nowIndex: 0
                }
            }
            return _createClass(ClFormula, [{
                key: "getValue",
                value: function (t, i) {
                    if (void 0 === t)
                        return 0;
                    if (void 0 === this.source.data || void 0 === this.source.data.value)
                        return 0;
                    void 0 === this.source.nowIndex && (this.source.nowIndex = this.source.minIndex);
                    var e = this.source.nowIndex - i;
                    return Object(_data_cl_data_tools__WEBPACK_IMPORTED_MODULE_0__.d)(this.source.data, t, e)
                }
            }, {
                key: "runSingleStock",
                value: function runSingleStock(source, formula) {
                    if (!eval) {
                        var _singleValue = []
                          , matchData = formula.match(/\.(\w+)(\(.+)/)
                          , funcName = matchData[1];
                        if ("function" != typeof this[funcName])
                            return _singleValue;
                        var argStr = matchData[2] || "";
                        for (argStr = argStr.replace(/\(|\)/g, "").split(","),
                        this.source = source,
                        this.source.nowIndex = this.source.minIndex; this.source.nowIndex <= this.source.maxIndex; this.source.nowIndex++) {
                            var out = this[funcName].apply(this, _toConsumableArray(argStr));
                            _singleValue.push([this.getValue("idx", 0), out])
                        }
                        return _singleValue
                    }
                    var singleValue = [];
                    this.source = source;
                    var command = "\n    for (this.source.nowIndex = this.source.minIndex;this.source.nowIndex <= this.source.maxIndex;this.source.nowIndex++) {\n          const ".concat(formula, "\n          singleValue.push([this.getValue('idx', 0), out]);\n    }");
                    return eval(command),
                    singleValue
                }
            }, {
                key: "MA",
                value: function (t, i) {
                    for (var e = 0, n = 0, a = 0; a < i; a++) {
                        var s = this.getValue(t, a);
                        0 !== s && (e++,
                        n += s)
                    }
                    return 0 === e ? 0 : n / e
                }
            }, {
                key: "AVGPRC",
                value: function () {
                    return this.getValue("allmoney", 0) / this.getValue("allvol", 0)
                }
            }]),
            ClFormula
        }()
    }
    , function (t, i, e) {
        "use strict";
        e.r(i);
        var n = {};
        e.r(n),
        e.d(n, "CHART_LAYOUT", function () {
            return $
        }),
        e.d(n, "CHART_BUTTONS", function () {
            return Q
        }),
        e.d(n, "CHART_ORDER", function () {
            return tt
        }),
        e.d(n, "CHART_KBAR", function () {
            return et
        }),
        e.d(n, "CHART_VBAR", function () {
            return nt
        }),
        e.d(n, "CHART_NOW", function () {
            return at
        }),
        e.d(n, "CHART_NOWVOL", function () {
            return st
        }),
        e.d(n, "CHART_DAY5", function () {
            return ot
        }),
        e.d(n, "CHART_DAY5VOL", function () {
            return ht
        }),
        e.d(n, "CHART_NORMAL", function () {
            return rt
        });
        var a = e(0);
        function s(t, i) {
            t.lineWidth = i
        }
        function o(t, i, e, n) {
            t.moveTo(i, e),
            t.lineTo(i, n)
        }
        function h(t, i, e, n) {
            t.moveTo(i, n),
            t.lineTo(e, n)
        }
        function r(t, i, e) {
            t.moveTo(i, e)
        }
        function l(t, i, e) {
            t.lineTo(i, e)
        }
        function c(t, i, e, n, a) {
            t.strokeRect(i, e, n, a)
        }
        function f(t, i, e, n, a, s) {
            t.fillStyle = s || t.fillStyle,
            t.fillRect(i, e, n, a)
        }
        function u(t, i) {
            t.beginPath(),
            t.strokeStyle = i || t.strokeStyle
        }
        function d(t) {
            t.stroke()
        }
        function v(t, i, e, n, a, s) {
            s = void 0 === s ? 5 : s;
            for (var o, h, r = (o = n - i,
            h = a - e,
            Math.sqrt(Math.pow(o, 2) + Math.pow(h, 2))), l = Math.floor(r / s), c = 0; c < l; c++)
                t[c % 2 == 0 ? "moveTo" : "lineTo"](i + (n - i) / l * c, e + (a - e) / l * c)
        }
        function m(t, i, e) {
            t.font = e + "px " + i
        }
        function x(t, i, e, n, a, s, o, h) {
            m(t, a, s),
            t.fillStyle = o || t.fillStyle,
            t.textBaseline = h && h.y || "top",
            t.textAlign = h && h.x || "start",
            t.fillText(n.toString(), i, e)
        }
        function y(t, i, e) {
            for (var n = e / 12, a = 0, s = 0; s < i.length; s++) {
                var o = i[s].toString();
                t && t[o] ? a += t[o].width : a += 12
            }
            return a * n
        }
        function g(t, i, e, n) {
            var a;
            if (m(t, e, n),
            t.measureText)
                try {
                    a = t.measureText(i).width
                } catch (e) {
                    a = y(t.charMap, i, n)
                }
            else
                a = y(t.charMap, i, n);
            return a
        }
        function p(t, i, e) {
            var n = e.spaceX || 2
              , a = e.spaceY || 2;
            return {
                width: g(t, i, e.font, e.pixel) + 2 * n,
                height: e.pixel + 2 * a
            }
        }
        function b(t, i, e, n, a) {
            var s, o, h = a.spaceX || 2, r = a.spaceY || 2, l = p(t, n, a);
            o = e,
            s = i,
            "middle" === a.y && (o = e - Math.floor(l.height / 2)),
            "end" === a.x && (s = i - l.width),
            "center" === a.x && (s = i - Math.floor(l.width / 2)),
            f(t, s, o, l.width, l.height, a.bakclr),
            u(t, a.clr),
            c(t, s, o, l.width, l.height),
            s = i,
            o = e,
            "start" === a.x && (s = i + h),
            "end" === a.x && (s = i - h),
            "top" === a.y && (o = e - r),
            x(t, s, o, n, a.font, a.pixel, a.clr, {
                x: a.x,
                y: a.y
            }),
            d(t)
        }
        function k(t, i, e, n) {
            t.moveTo(i + n, e),
            t.arc(i, e, n, 0, 2 * Math.PI, !0)
        }
        function w(t, i, e, n, a) {
            t.moveTo(i + n, e),
            t.arc(i, e, n, 0, 2 * Math.PI, !0),
            t.fillStyle = a,
            t.fill()
        }
        function I(t, i, e, n, a, s) {
            var o = t.strokeStyle;
            u(t, s),
            t.moveTo(i, e),
            t.lineTo(n, a),
            d(t),
            t.strokeStyle = o
        }
        function M(t, i, e, n, a, s) {
            void 0 !== n && 0 < n.r && (u(t, n.clr),
            w(t, i, e, n.r, n.clr),
            d(t)),
            void 0 !== a && 0 < a.r && (u(t, a.clr),
            w(t, i, e, a.r, a.clr),
            d(t)),
            void 0 !== s && 0 < s.r && (u(t, s.clr),
            w(t, i, e, s.r, s.clr),
            d(t))
        }
        function O(t, i, e) {
            u(t, i.clr),
            v(t, i.xx, i.yy, i.right - i.pixel / 2, i.yy, 7),
            d(t);
            var n = i.spaceX || 2 * i.linew
              , a = i.spaceY || i.linew;
            i.width = i.right - i.xx;
            for (var s = 0; s < e.length; s++)
                if ("false" !== e[s].display) {
                    var o = i.xx + i.width * e[s].set / 100;
                    if ("arc" === e[s].txt)
                        o + e[s].maxR > i.right && (o = i.right - e[s].maxR),
                        M(t, o, i.yy, {
                            r: e[s].maxR,
                            clr: i.clr
                        }, {
                            r: e[s].minR,
                            clr: i.bakclr
                        });
                    else {
                        var h = p(t, e[s].txt, {
                            font: i.font,
                            pixel: i.pixel,
                            spaceX: n,
                            spaceY: a
                        });
                        o + h.width > i.right && (o = i.right - h.width);
                        var r = i.yy;
                        i.top && r < i.top + h.height / 2 && (r = i.top + h.height / 2),
                        i.bottom && r > i.bottom - h.height / 2 && (r = i.bottom - h.height / 2),
                        b(t, o, r, e[s].txt, {
                            font: i.font,
                            pixel: i.pixel,
                            clr: i.clr,
                            bakclr: i.bakclr,
                            x: "start",
                            y: i.y,
                            spaceX: n,
                            spaceY: a
                        })
                    }
                }
        }
        function D(t, i, e) {
            var n, a = i.rect.left + i.index * (i.unitX + i.spaceX), s = a + Math.floor(i.unitX / 2), r = i.rect.top + Math.round((i.maxmin.max - e[1]) * i.unitY), l = i.rect.top + i.rect.height - Math.round((e[2] - i.maxmin.min) * i.unitY), u = i.rect.top + Math.round((i.maxmin.max - e[0]) * i.unitY);
            e[0] === e[3] ? (n = 0,
            h(t, a, a + i.unitX, u),
            e[1] > e[2] && o(t, s, r, l)) : (n = Math.round((e[0] - e[3]) * i.unitY),
            o(t, s, r, u),
            i.filled ? f(t, a, u, i.unitX, n, i.fillclr) : c(t, a, u, i.unitX, n),
            o(t, s, u + n, l))
        }
        function C(t, i, e) {
            var n = i.rect.left + i.index * (i.unitX + i.spaceX)
              , a = i.rect.top + Math.round((i.maxmin.max - e) * i.unitY)
              , s = i.rect.top + i.rect.height - a;
            i.filled ? f(t, n, a, i.unitX, s, i.fillclr) : c(t, n, a, i.unitX, s)
        }
        function j(t, i, e) {
            var n = t.toLowerCase();
            if (n && /^#([0-9a-fA-f]{3}|[0-9a-fA-f]{6})$/.test(n)) {
                if (4 === n.length) {
                    for (var a = "#", s = 1; s < 4; s += 1)
                        a += n.slice(s, s + 1).concat(n.slice(s, s + 1));
                    n = a
                }
                for (var o = [], h = 1; h < 7; h += 2)
                    o.push(parseInt("0x" + n.slice(h, h + 2)));
                switch (e) {
                    case "adjust":
                        var r = o[0]
                          , l = o[1]
                          , c = o[2];
                        o[0] = .272 * r + .534 * l + .131 * c,
                        o[1] = .349 * r + .686 * l + .168 * c,
                        o[2] = .393 * r + .769 * l + .189 * c
                }
                return "rgba(" + (n = o.join(",")) + "," + (i = i || 1) + ")"
            }
            return n
        }
        var T = e(1);
        function X(t, i, e, n) {
            if (void 0 === e && (e = "time"),
            n && "forword" === n) {
                for (var a = t.value.length - 1; 0 <= a; a--)
                    if (!(Object(T.d)(t, e, a) > i))
                        return a
            } else
                for (var s = 0; s <= t.value.length - 1; s++)
                    if (!(Object(T.d)(t, e, s) < i))
                        return s;
            return -1
        }
        function Y(t, i) {
            t.showMode = "fixed",
            t.fixed = {
                min: 0,
                max: i.maxCount - 1,
                left: 0,
                right: 0
            },
            t.minIndex = 0,
            t.maxIndex = i.size - 1,
            t.maxCount = 1 < i.maxCount ? i.maxCount : 2,
            t.unitX = i.scale,
            t.spaceX = i.width / t.maxCount - t.unitX
        }
        var L = {
            sys: "white",
            line: ["#4048cc", "#dd8d2d", "#168ee0", "#ad7eac", "#ff8290", "#ba1215"],
            back: "#fafafa",
            txt: "#2f2f2f",
            baktxt: "#2f2f2f",
            axis: "#7f7f7f",
            grid: "#cccccc",
            red: "#ff6a6c",
            green: "#32cb47",
            white: "#7e7e7e",
            vol: "#dd8d2d",
            button: "#888888",
            colume: "#41bfd0",
            box: "#ddf4df",
            code: "#3f3f3f"
        }
          , E = {
              sys: "black",
              line: ["#efefef", "#fdc104", "#5bbccf", "#ad7eac", "#bf2f2f", "#cfcfcf"],
              back: "#232323",
              txt: "#bfbfbf",
              baktxt: "#2f2f2f",
              axis: "#afafaf",
              grid: "#373737",
              red: "#ff6a6c",
              green: "#6ad36d",
              white: "#9f9f9f",
              vol: "#fdc104",
              button: "#9d9d9d",
              colume: "#41bfd0",
              box: "#373737",
              code: "#41bfd0"
          }
          , _ = {
              runPlatform: "normal",
              axisPlatform: "web",
              eventPlatform: "web",
              scale: 1,
              standard: "china",
              sysColor: "black",
              charMap: {
                  0: {
                      width: 7.1999969482421875
                  },
                  1: {
                      width: 4.8119964599609375
                  },
                  2: {
                      width: 7.1999969482421875
                  },
                  3: {
                      width: 7.1999969482421875
                  },
                  4: {
                      width: 7.1999969482421875
                  },
                  5: {
                      width: 7.1999969482421875
                  },
                  6: {
                      width: 7.1999969482421875
                  },
                  7: {
                      width: 6.563995361328125
                  },
                  8: {
                      width: 7.1999969482421875
                  },
                  9: {
                      width: 7.1999969482421875
                  },
                  ".": {
                      width: 3.167999267578125
                  },
                  ",": {
                      width: 3.167999267578125
                  },
                  "%": {
                      width: 11.639999389648438
                  },
                  ":": {
                      width: 3.167999267578125
                  },
                  " ": {
                      width: 3.9959869384765625
                  },
                  K: {
                      width: 8.279998779296875
                  },
                  V: {
                      width: 7.667999267578125
                  },
                  O: {
                      width: 9.203994750976562
                  },
                  L: {
                      width: 7.055999755859375
                  },
                  "-": {
                      width: 7.2599945068359375
                  },
                  "[": {
                      width: 3.9959869384765625
                  },
                  "]": {
                      width: 3.9959869384765625
                  }
              },
              mainCanvas: {},
              cursorCanvas: {}
          };
        function S(t) {
            var i = {};
            if (i = "white" === t ? Object(a.b)(L) : Object(a.b)(E),
            "usa" === _.standard) {
                var e = i.red;
                i.red = i.green,
                i.green = e
            }
            return _.color = i
        }
        function A(t, i) {
            return t.width = t.clientWidth * i,
            t.height = t.clientHeight * i,
            {
                width: t.width,
                height: t.height
            }
        }
        function P(t) {
            if (void 0 !== t) {
                for (var i in _)
                    _[i] = t[i] || _[i];
                return _.mainCanvas.canvas = t.mainCanvas.canvas,
                _.mainCanvas.context = t.mainCanvas.context,
                _.mainCanvas.context.charMap = _.charMap,
                _.cursorCanvas.canvas = t.cursorCanvas.canvas,
                _.cursorCanvas.context = t.cursorCanvas.context,
                _.cursorCanvas.context.charMap = _.charMap,
                _.color = S(_.sysColor),
                "normal" === _.runPlatform && void 0 !== _.mainCanvas.canvas && 1 !== _.scale && (A(_.mainCanvas.canvas, _.scale),
                A(_.cursorCanvas.canvas, _.scale)),
                _
            }
        }
        function F(t, i) {
            t.father = i,
            t.context = i.context,
            t.scale = _.scale,
            t.color = _.color,
            t.axisPlatform = _.axisPlatform,
            t.eventPlatform = _.eventPlatform
        }
        function R(t) {
            var i = _.scale;
            t.margin.top *= i,
            t.margin.left *= i,
            t.margin.bottom *= i,
            t.margin.right *= i,
            t.offset.top *= i,
            t.offset.left *= i,
            t.offset.bottom *= i,
            t.offset.right *= i,
            t.title.pixel *= i,
            t.title.height *= i,
            t.title.spaceX *= i,
            t.title.spaceY *= i,
            t.title.height < t.title.pixel + t.title.spaceY + 2 * i && (t.title.height = t.title.pixel + t.title.spaceY + 2 * i),
            t.axisX.pixel *= i,
            t.axisX.width *= i,
            t.axisX.height *= i,
            t.axisX.spaceX *= i,
            t.scroll.pixel *= i,
            t.scroll.size *= i,
            t.scroll.spaceX *= i,
            t.digit.pixel *= i,
            t.digit.height *= i,
            t.digit.spaceX *= i,
            t.digit.height < t.digit.pixel + 2 * i && (t.digit.height = t.digit.pixel + 2 * i),
            t.symbol.size *= i,
            t.symbol.spaceX *= i,
            t.symbol.spaceY *= i
        }
        function N(t) {
            "html5" === _.eventPlatform && (_.mainCanvas.canvas.style.cursor = t,
            _.cursorCanvas.canvas.style.cursor = t)
        }
        function z(t) {
            return void 0 === t && (t = 0),
            _.color.line[t % _.color.line.length]
        }
        function H(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var K = function () {
            function t(i, e) {
                !function (i, e) {
                    if (!(i instanceof t))
                        throw new TypeError("Cannot call a class as a function")
                }(this),
                F(this, i),
                this.rectMain = e,
                this.linkInfo = i.father.linkInfo,
                this.source = i.father,
                this.maxmin = i.maxmin
            }
            var i, e;
            return i = t,
            (e = [{
                key: "onPaint",
                value: function (t) {
                    void 0 !== t && (this.hotKey = t),
                    this.data = this.source.getData(this.hotKey),
                    this.codeInfo = this.source.getData("INFO");
                    var i, e, n = this.color.red;
                    u(this.context, n);
                    for (var a = Object(T.d)(this.codeInfo, "before", 0), s = 0, o = this.linkInfo.minIndex; o <= this.linkInfo.maxIndex; s++,
                    o++)
                        0 < o && (a = Object(T.d)(this.data, "close", o - 1)),
                        ((i = Object(T.d)(this.data, "open", o)) < (e = Object(T.d)(this.data, "close", o)) || i === e && a <= e) && D(this.context, {
                            filled: "white" === this.color.sys,
                            index: s,
                            spaceX: this.linkInfo.spaceX,
                            unitX: this.linkInfo.unitX,
                            unitY: this.maxmin.unitY,
                            maxmin: this.maxmin,
                            rect: this.rectMain,
                            fillclr: n
                        }, [e, Object(T.d)(this.data, "high", o), Object(T.d)(this.data, "low", o), i]);
                    d(this.context),
                    n = this.color.green,
                    u(this.context, n);
                    for (var h = 0, r = this.linkInfo.minIndex; r <= this.linkInfo.maxIndex; h++,
                    r++)
                        0 < r && (a = Object(T.d)(this.data, "close", r - 1)),
                        i = Object(T.d)(this.data, "open", r),
                        ((e = Object(T.d)(this.data, "close", r)) < i || i === e && e < a) && D(this.context, {
                            filled: !0,
                            index: h,
                            spaceX: this.linkInfo.spaceX,
                            unitX: this.linkInfo.unitX,
                            unitY: this.maxmin.unitY,
                            maxmin: this.maxmin,
                            rect: this.rectMain,
                            fillclr: n
                        }, [i, Object(T.d)(this.data, "high", r), Object(T.d)(this.data, "low", r), e]);
                    d(this.context)
                }
            }]) && H(i.prototype, e),
            t
        }();
        function B(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var W = function () {
            function t(i, e) {
                !function (i, e) {
                    if (!(i instanceof t))
                        throw new TypeError("Cannot call a class as a function")
                }(this),
                F(this, i),
                this.rectMain = e,
                this.linkInfo = i.father.linkInfo,
                this.source = i.father,
                this.maxmin = i.maxmin
            }
            var i, e;
            return i = t,
            (e = [{
                key: "onPaint",
                value: function (t) {
                    var i, e;
                    void 0 !== t && (this.hotKey = t),
                    this.data = this.source.getData(this.hotKey),
                    void 0 === this.info.labelX && (this.info.labelX = "idx"),
                    void 0 === this.info.labelY && (this.info.labelY = "value");
                    var n, s, o = !1, h = 0;
                    s = void 0 === this.info.color ? z(this.info.colorIndex) : this.color[this.info.color],
                    u(this.context, s);
                    for (var c = this.linkInfo.minIndex, f = 0; c <= this.linkInfo.maxIndex; c++,
                    f++)
                        n = void 0 === this.info.showSort ? f : Object(T.d)(this.data, this.info.showSort, f),
                        i = this.rectMain.left + n * (this.linkInfo.unitX + this.linkInfo.spaceX),
                        e = this.rectMain.top + Math.round((this.maxmin.max - Object(T.d)(this.data, this.info.labelY, f)) * this.maxmin.unitY),
                        Math.floor(n / this.info.skips) > h && (h = Math.floor(n / this.info.skips),
                        o = !1),
                        o ? l(this.context, i, e) : (o = Object(a.r)(this.rectMain, {
                            x: i,
                            y: e
                        })) && r(this.context, i, e);
                    d(this.context)
                }
            }]) && B(i.prototype, e),
            t
        }();
        function G(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var V = function () {
            function t(i, e) {
                !function (i, e) {
                    if (!(i instanceof t))
                        throw new TypeError("Cannot call a class as a function")
                }(this),
                F(this, i),
                this.rectMain = e,
                this.linkInfo = i.father.linkInfo,
                this.source = i.father,
                this.symbol = i.layout.symbol
            }
            var i, e;
            return i = t,
            (e = [{
                key: "onPaint",
                value: function (t) {
                    if (void 0 !== t && (this.hotKey = t),
                    this.data = this.source.getData(this.hotKey),
                    this.rightData = this.source.getData("RIGHT"),
                    !(Object(T.k)(this.rightData) < 1))
                        for (var i = g(this.context, "▲", this.symbol.font, this.symbol.pixel), e = 0; e < this.rightData.value.length; e++) {
                            var n = X(this.data, Object(T.d)(this.rightData, "time", e)) - this.linkInfo.minIndex
                              , a = this.rectMain.left + n * (this.linkInfo.unitX + this.linkInfo.spaceX) + Math.floor(this.linkInfo.unitX / 2);
                            if (!(a < this.rectMain.left)) {
                                var s = this.color.button;
                                "no" !== this.linkInfo.rightMode && (s = this.color.vol),
                                x(this.context, a - Math.floor(i / 2), this.rectMain.top + this.rectMain.height - this.symbol.pixel - this.symbol.spaceY, "▲", this.symbol.font, this.symbol.pixel, s, {
                                    y: "top"
                                })
                            }
                        }
                }
            }]) && G(i.prototype, e),
            t
        }();
        function U(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var q = function () {
            function t(i, e) {
                !function (i, e) {
                    if (!(i instanceof t))
                        throw new TypeError("Cannot call a class as a function")
                }(this),
                F(this, i),
                this.rectMain = e,
                this.linkInfo = i.father.linkInfo,
                this.source = i.father,
                this.maxmin = i.maxmin
            }
            var i, e;
            return i = t,
            (e = [{
                key: "onPaint",
                value: function (t) {
                    void 0 !== t && (this.hotKey = t),
                    this.data = this.source.getData(this.hotKey),
                    u(this.context, this.color.red);
                    for (var i = 0, e = this.linkInfo.minIndex; e <= this.linkInfo.maxIndex; i++,
                    e++)
                        parseFloat(Object(T.d)(this.data, "open", e)) <= parseFloat(Object(T.d)(this.data, "close", e)) && C(this.context, {
                            filled: "white" === this.color.sys,
                            index: i,
                            spaceX: this.linkInfo.spaceX,
                            unitX: this.linkInfo.unitX,
                            unitY: this.maxmin.unitY,
                            maxmin: this.maxmin,
                            rect: this.rectMain,
                            fillclr: this.color.red
                        }, Object(T.d)(this.data, "vol", e));
                    d(this.context),
                    u(this.context, this.color.green);
                    for (var n = 0, a = this.linkInfo.minIndex; a <= this.linkInfo.maxIndex; n++,
                    a++)
                        parseFloat(Object(T.d)(this.data, "open", a)) > parseFloat(Object(T.d)(this.data, "close", a)) && C(this.context, {
                            filled: !0,
                            index: n,
                            spaceX: this.linkInfo.spaceX,
                            unitX: this.linkInfo.unitX,
                            unitY: this.maxmin.unitY,
                            maxmin: this.maxmin,
                            rect: this.rectMain,
                            fillclr: this.color.green
                        }, Object(T.d)(this.data, "vol", a));
                    d(this.context)
                }
            }]) && U(i.prototype, e),
            t
        }();
        function Z(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var J = function () {
            function t(i, e) {
                !function (i, e) {
                    if (!(i instanceof t))
                        throw new TypeError("Cannot call a class as a function")
                }(this),
                F(this, i),
                this.rectMain = e,
                this.linkInfo = i.father.linkInfo,
                this.source = i.father,
                this.maxmin = i.maxmin
            }
            var i, e;
            return i = t,
            (e = [{
                key: "onPaint",
                value: function (t) {
                    var i, e, n, a, s;
                    void 0 !== t && (this.hotKey = t),
                    this.data = this.source.getData(this.hotKey),
                    void 0 === this.info.labelX && (this.info.labelX = "time"),
                    void 0 === this.info.labelY && (this.info.labelY = "vol"),
                    s = void 0 === this.info.color ? z(this.info.colorIndex) : this.color[this.info.color],
                    u(this.context, s);
                    for (var o = this.linkInfo.minIndex, h = 0; o <= this.linkInfo.maxIndex; o++,
                    h++)
                        a = void 0 === this.info.showSort ? h : Object(T.d)(this.data, this.info.showSort, h),
                        i = this.rectMain.left + Math.floor(a * (this.linkInfo.spaceX + this.linkInfo.unitX)),
                        (n = Object(T.d)(this.data, this.info.labelY, o)) < 0 || ((e = this.rectMain.top + Math.round((this.maxmin.max - n) * this.maxmin.unitY)) < this.rectMain.top ? (e = this.rectMain.top + 1,
                        I(this.context, i, this.rectMain.top + this.rectMain.height - 1, i, e, this.color.white)) : (r(this.context, i, this.rectMain.top + this.rectMain.height - 1),
                        l(this.context, i, e)));
                    d(this.context)
                }
            }]) && Z(i.prototype, e),
            t
        }()
          , $ = {
              margin: {
                  left: 0,
                  top: 0,
                  right: 0,
                  bottom: 0
              },
              offset: {
                  left: 2,
                  top: 2,
                  right: 2,
                  bottom: 0
              },
              title: {
                  pixel: 12,
                  height: 18,
                  spaceX: 10,
                  spaceY: 2,
                  font: "sans-serif"
              },
              axisX: {
                  pixel: 12,
                  height: 18,
                  width: 50,
                  spaceX: 2,
                  font: "sans-serif"
              },
              scroll: {
                  pixel: 12,
                  size: 15,
                  spaceX: 10,
                  font: "sans-serif"
              },
              digit: {
                  pixel: 12,
                  height: 16,
                  spaceX: 3,
                  font: "sans-serif"
              },
              symbol: {
                  pixel: 10,
                  size: 18,
                  spaceX: 3,
                  font: "sans-serif"
              }
          }
          , Q = [{
              key: "zoomin"
          }, {
              key: "zoomout"
          }, {
              key: "exright"
          }]
          , tt = {
              style: "normal",
              title: {
                  display: "text"
              }
          }
          , it = {
              index: 3,
              list: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
          }
          , et = {
              zoomInfo: it,
              scroll: {
                  display: "none"
              },
              title: {
                  display: "text",
                  label: "K线"
              },
              axisX: {
                  lines: 0,
                  display: "none",
                  type: "normal",
                  format: "date"
              },
              axisY: {
                  lines: 3,
                  left: {
                      display: "both",
                      middle: "none",
                      format: "price"
                  },
                  right: {
                      display: "both",
                      middle: "none",
                      format: "price"
                  }
              },
              lines: [{
                  className: K,
                  extremum: {
                      method: "normal",
                      maxvalue: ["high"],
                      minvalue: ["low"]
                  }
              }, {
                  className: V
              }, {
                  className: W,
                  info: {
                      txt: "5:",
                      labelY: "value",
                      format: "price"
                  },
                  formula: {
                      key: "DAYM1",
                      command: "out = this.MA('close',5)"
                  }
              }, {
                  className: W,
                  info: {
                      txt: "10:",
                      labelY: "value",
                      format: "price"
                  },
                  formula: {
                      key: "DAYM2",
                      command: "out = this.MA('close',10)"
                  }
              }, {
                  className: W,
                  info: {
                      txt: "20:",
                      labelY: "value",
                      format: "price"
                  },
                  formula: {
                      key: "DAYM3",
                      command: "out = this.MA('close',20)"
                  }
              }, {
                  className: W,
                  info: {
                      txt: "60:",
                      labelY: "value",
                      format: "price"
                  },
                  formula: {
                      key: "DAYM4",
                      command: "out = this.MA('close',60)"
                  }
              }]
          }
          , nt = {
              zoomInfo: it,
              title: {
                  display: "text",
                  label: "VOL"
              },
              axisX: {
                  lines: 0,
                  display: "both",
                  type: "normal",
                  format: "date"
              },
              axisY: {
                  lines: 1,
                  left: {
                      display: "nofoot",
                      middle: "none",
                      format: "vol"
                  },
                  right: {
                      display: "nofoot",
                      middle: "none",
                      format: "vol"
                  }
              },
              lines: [{
                  className: q,
                  extremum: {
                      method: "normal",
                      maxvalue: ["vol"],
                      minvalue: [0]
                  },
                  info: {
                      labelY: "vol",
                      format: "vol"
                  }
              }, {
                  className: W,
                  info: {
                      txt: "5:",
                      labelY: "value",
                      format: "vol"
                  },
                  formula: {
                      key: "MVOL1",
                      command: "out = this.MA('vol',5)"
                  }
              }, {
                  className: W,
                  info: {
                      txt: "10:",
                      labelY: "value",
                      format: "vol"
                  },
                  formula: {
                      key: "MVOL2",
                      command: "out = this.MA('vol',10)"
                  }
              }, {
                  className: W,
                  info: {
                      txt: "20:",
                      labelY: "value",
                      format: "vol"
                  },
                  formula: {
                      key: "MVOL3",
                      command: "out = this.MA('vol',20)"
                  }
              }]
          }
          , at = {
              title: {
                  display: "none"
              },
              axisX: {
                  lines: 3,
                  display: "none",
                  type: "day1",
                  format: "tradetime"
              },
              axisY: {
                  lines: 3,
                  left: {
                      display: "both",
                      middle: "before",
                      format: "price"
                  },
                  right: {
                      display: "both",
                      middle: "before",
                      format: "rate"
                  }
              },
              lines: [{
                  className: W,
                  extremum: {
                      method: "fixedLeft",
                      maxvalue: ["high"],
                      minvalue: ["low"]
                  },
                  info: {
                      labelX: "idx",
                      labelY: "close",
                      showSort: "idx"
                  }
              }, {
                  className: W,
                  info: {
                      showSort: "idx"
                  },
                  formula: {
                      key: "NOWM1",
                      command: "out = this.AVGPRC()"
                  }
              }]
          }
          , st = {
              title: {
                  display: "none"
              },
              axisX: {
                  lines: 3,
                  display: "both",
                  type: "day1",
                  format: "tradetime"
              },
              axisY: {
                  lines: 1,
                  left: {
                      display: "nofoot",
                      middle: "none",
                      format: "vol"
                  },
                  right: {
                      display: "nofoot",
                      middle: "none",
                      format: "vol"
                  }
              },
              lines: [{
                  className: J,
                  extremum: {
                      method: "normal",
                      maxvalue: ["decvol"],
                      minvalue: [0]
                  },
                  info: {
                      showSort: "idx",
                      labelX: "idx",
                      labelY: "decvol",
                      color: "vol"
                  }
              }]
          }
          , ot = {
              title: {
                  display: "none"
              },
              axisX: {
                  lines: 4,
                  display: "none",
                  type: "day5",
                  format: "date"
              },
              axisY: {
                  lines: 3,
                  left: {
                      display: "both",
                      middle: "before",
                      format: "price"
                  },
                  right: {
                      display: "both",
                      middle: "before",
                      format: "rate"
                  }
              },
              lines: [{
                  className: W,
                  extremum: {
                      method: "fixedLeft",
                      maxvalue: ["close"],
                      minvalue: ["close"]
                  },
                  info: {
                      showSort: "idx",
                      labelX: "time",
                      labelY: "close"
                  }
              }, {
                  className: W,
                  info: {
                      showSort: "idx"
                  },
                  formula: {
                      key: "NOWDAY5",
                      command: "out = this.AVGPRC()"
                  }
              }]
          }
          , ht = {
              title: {
                  display: "none"
              },
              axisX: {
                  lines: 4,
                  display: "block",
                  type: "day5",
                  format: "date"
              },
              axisY: {
                  lines: 1,
                  left: {
                      display: "nofoot",
                      middle: "none",
                      format: "vol"
                  },
                  right: {
                      display: "nofoot",
                      middle: "none",
                      format: "vol"
                  }
              },
              lines: [{
                  className: J,
                  extremum: {
                      method: "normal",
                      maxvalue: ["vol"],
                      minvalue: [0]
                  },
                  info: {
                      showSort: "idx",
                      labelX: "time",
                      labelY: "vol",
                      color: "vol"
                  }
              }]
          }
          , rt = {
              title: {
                  display: "text",
                  label: "NORMAL"
              },
              axisX: {
                  lines: 0,
                  display: "none",
                  type: "normal",
                  format: "date"
              },
              axisY: {
                  lines: 1,
                  left: {
                      display: "both",
                      middle: "none",
                      format: "price"
                  },
                  right: {
                      display: "both",
                      middle: "none",
                      format: "price"
                  }
              },
              lines: [{
                  className: W
              }]
          }
          , lt = e(2);
        function ct(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var ft = {
            shape: "arc",
            hotIdx: 0,
            visible: !0,
            translucent: !0,
            status: "enabled"
        }
          , ut = function () {
              function t(i) {
                  !function (t, i) {
                      if (!(t instanceof i))
                          throw new TypeError("Cannot call a class as a function")
                  }(this, t),
                  F(this, i)
              }
              var i, e;
              return i = t,
              (e = [{
                  key: "init",
                  value: function (t, i) {
                      this.callback = i,
                      this.rectMain = t.rectMain || {
                          left: 0,
                          top: 0,
                          width: 25,
                          height: 25
                      },
                      this.layout = Object(a.v)(t.layout, $),
                      this.config = Object(a.v)(t.config, ft),
                      this.info = t.info || [{
                          map: "+"
                      }],
                      this.checkConfig()
                  }
              }, {
                  key: "checkConfig",
                  value: function () {
                      R(this.layout)
                  }
              }, {
                  key: "setStatus",
                  value: function (t) {
                      this.config.status !== t && (this.config.status = t)
                  }
              }, {
                  key: "onClick",
                  value: function (t) {
                      this.config.visible && (1 < this.info.length && (this.config.hotIdx++,
                      this.config.hotIdx %= this.info.length,
                      this.onPaint()),
                      0 <= this.config.hotIdx && this.config.hotIdx < this.info.length ? this.callback({
                          self: this,
                          info: this.info[this.config.hotIdx]
                      }) : this.callback({
                          self: this
                      }),
                      t.break = !0)
                  }
              }, {
                  key: "onPaint",
                  value: function () {
                      if (this.config.visible) {
                          s(this.context, this.scale);
                          var t = this.color.button;
                          "disabled" === this.config.status && (t = this.color.grid),
                          u(this.context, t);
                          var i, e, n, a, v, m, y = {
                              xx: Math.floor(this.rectMain.width / 2),
                              yy: Math.floor(this.rectMain.height / 2),
                              offset: 4 * this.scale
                          }, g = this.info[this.config.hotIdx];
                          switch (this.config.shape) {
                              case "set":
                                  "focused" === this.config.status ? (t = this.color.red,
                                  this.config.translucent && (t = j(t, .95)),
                                  i = this.context,
                                  e = this.rectMain.left + y.xx,
                                  n = this.rectMain.top + y.xx,
                                  void 0 !== (a = {
                                      r: Math.round(this.layout.symbol.size / 2),
                                      clr: t
                                  }) && 0 < a.r && (u(i, a.clr),
                                  r(i, e - a.r, n),
                                  l(i, e + a.r, n),
                                  l(i, e, n + 2 * a.r),
                                  l(i, e - a.r, n),
                                  v = i,
                                  m = a.clr,
                                  v.fillStyle = m,
                                  v.fill(),
                                  w(i, e, n, a.r, a.clr),
                                  d(i)),
                                  y.yy = y.xx) : (t = this.color.vol,
                                  this.config.translucent && (t = j(t, .85)),
                                  M(this.context, this.rectMain.left + y.xx, this.rectMain.top + y.xx, {
                                      r: Math.round(this.layout.symbol.size / 2),
                                      clr: t
                                  }));
                                  break;
                              case "arc":
                                  k(this.context, this.rectMain.left + y.xx, this.rectMain.top + y.xx, y.xx);
                                  break;
                              case "box":
                                  f(this.context, this.rectMain.left, this.rectMain.top, this.rectMain.width, this.rectMain.height, this.color.back),
                                  c(this.context, this.rectMain.left, this.rectMain.top, this.rectMain.width, this.rectMain.height);
                                  break;
                              case "range":
                              case "radio":
                              case "checkbox":
                                  k(this.context, this.rectMain.left + y.xx, this.rectMain.top + y.xx, y.xx)
                          }
                          switch (d(this.context),
                          "disabled" === this.config.status ? u(this.context, this.color.grid) : u(this.context, this.color.button),
                          g.map) {
                              case "+":
                                  o(this.context, this.rectMain.left + y.xx, this.rectMain.top + y.offset, this.rectMain.top + this.rectMain.height - y.offset),
                                  h(this.context, this.rectMain.left + y.offset, this.rectMain.left + this.rectMain.width - y.offset, this.rectMain.top + y.yy);
                                  break;
                              case "-":
                                  h(this.context, this.rectMain.left + y.offset, this.rectMain.left + this.rectMain.width - y.offset, this.rectMain.top + y.yy);
                                  break;
                              case "0":
                              case "1":
                              case "2":
                              case "3":
                              case "4":
                              case "5":
                              case "6":
                              case "7":
                              case "8":
                              case "9":
                                  x(this.context, this.rectMain.left + y.xx, this.rectMain.top + y.yy, g.map, this.layout.title.font, this.layout.title.pixel, this.color.baktxt, {
                                      x: "center",
                                      y: "middle"
                                  });
                                  break;
                              case "*":
                                  x(this.context, this.rectMain.left + y.xx, this.rectMain.top + y.yy - 2 * this.scale, "...", this.layout.title.font, this.layout.title.pixel, this.color.baktxt, {
                                      x: "center",
                                      y: "middle"
                                  });
                                  break;
                              case "left":
                              case "right":
                                  break;
                              default:
                                  x(this.context, this.rectMain.left + y.xx, this.rectMain.top + y.yy, g.map, this.layout.title.font, this.layout.title.pixel, this.color.button, {
                                      x: "center",
                                      y: "middle"
                                  })
                          }
                          d(this.context)
                      }
                  }
              }]) && ct(i.prototype, e),
              t
          }();
        function dt(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var vt = {
            shape: "fixed",
            direct: "horizontal",
            range: 100,
            select: {
                min: 40,
                max: 60
            },
            status: "enabled",
            txt: {}
        }
          , mt = function () {
              function t(i) {
                  !function (t, i) {
                      if (!(t instanceof i))
                          throw new TypeError("Cannot call a class as a function")
                  }(this, t),
                  F(this, i)
              }
              var i, e;
              return i = t,
              (e = [{
                  key: "init",
                  value: function (t, i) {
                      this.callback = i,
                      this.rectMain = t.rectMain || {
                          left: 0,
                          top: 0,
                          width: 200,
                          height: 25
                      },
                      this.layout = Object(a.v)(t.layout, $),
                      this.config = Object(a.v)(t.config, vt),
                      this.checkConfig(),
                      this.setPublicRect()
                  }
              }, {
                  key: "checkConfig",
                  value: function () {
                      R(this.layout)
                  }
              }, {
                  key: "setPublicRect",
                  value: function () {
                      var t, i, e, n, s = this.config.range;
                      this.rectChart = Object(a.u)(this.rectMain, this.layout.margin),
                      "horizontal" === this.config.direct && (t = this.rectMain.width / (s - 1),
                      n = this.rectMain.height - 2 * this.scale,
                      i = this.rectMain.left + t * this.config.select.min,
                      e = this.rectMain.left + t * this.config.select.max,
                      "free" === this.config.shape ? (this.rectMin = {
                          left: i - this.layout.scroll.size / 2,
                          top: this.rectMain.top + this.scale,
                          width: this.layout.scroll.size,
                          height: n
                      },
                      this.rectMax = {
                          left: e - this.layout.scroll.size / 2,
                          top: this.rectMain.top + this.scale,
                          width: this.layout.scroll.size,
                          height: n
                      },
                      this.rectMid = {
                          left: i + this.layout.scroll.size / 2,
                          top: this.rectMain.top + this.scale,
                          width: e - i - this.layout.scroll.size,
                          height: n
                      }) : this.rectMid = {
                          left: i,
                          top: this.rectMain.top + this.scale,
                          width: e - i,
                          height: n
                      })
                  }
              }, {
                  key: "onPaint",
                  value: function () {
                      s(this.context, this.scale),
                      this.drawClear(),
                      this.drawGridline(),
                      this.setPublicRect(),
                      this.drawButton()
                  }
              }, {
                  key: "onChange",
                  value: function (t) {
                      this.config = Object(a.v)(t, this.config),
                      this.config.select.max > this.config.range && (this.config.select.max = this.config.range - 1),
                      t.iscall && this.callback({
                          self: this,
                          minIndex: this.config.select.min
                      })
                  }
              }, {
                  key: "findMouseIndex",
                  value: function (t) {
                      var i = this.config.range
                        , e = this.rectMain.width / (i - 1);
                      return Math.round((t - this.rectMain.left) / e)
                  }
              }, {
                  key: "checkMin",
                  value: function (t) {
                      return t < 0 ? 0 : t > this.config.range - (this.config.select.max - this.config.select.min + 1) ? this.config.range - (this.config.select.max - this.config.select.min + 1) : t
                  }
              }, {
                  key: "onInit",
                  value: function () {
                      N("default"),
                      this.who = void 0
                  }
              }, {
                  key: "onMouseOut",
                  value: function () {
                      N("default"),
                      this.who = void 0
                  }
              }, {
                  key: "onMouseDown",
                  value: function (t) {
                      var i = t.mousePos;
                      Object(a.r)(this.rectMin, i) ? this.who = "min" : Object(a.r)(this.rectMax, i) ? this.who = "max" : Object(a.r)(this.rectMid, i) ? (this.who = "mid",
                      this.index = this.findMouseIndex(i.x)) : this.who = void 0
                  }
              }, {
                  key: "onMouseUp",
                  value: function (t) {
                      if ("free" !== this.config.shape && void 0 === this.who) {
                          var i = t.mousePos
                            , e = this.findMouseIndex(i.x) - Math.floor((this.config.select.max - this.config.select.min) / 2);
                          e = this.checkMin(e),
                          this.onChange({
                              min: e,
                              iscall: !0
                          })
                      }
                      this.who = void 0
                  }
              }, {
                  key: "onMouseMove",
                  value: function (t) {
                      var i = t.mousePos;
                      if (Object(a.r)(this.rectMin, i) || Object(a.r)(this.rectMax, i) ? N("col-resize") : Object(a.r)(this.rectMid, i) ? N("move") : N("default"),
                      void 0 !== this.who) {
                          var e, n, s = this.findMouseIndex(i.x);
                          "free" === this.config.shape ? (s < this.config.select.max ? "max" === this.who ? n = s : "min" === this.who && (e = s) : s >= this.config.select.max && ("min" === this.who ? (e = this.config.select.max,
                          n = s,
                          this.who = "max") : "max" === this.who && (n = s)),
                          this.onChange({
                              min: e,
                              max: n,
                              iscall: !0
                          })) : (e = this.config.select.min + s - this.index,
                          this.index = s,
                          e = this.checkMin(e),
                          this.onChange({
                              min: e,
                              iscall: !0
                          }))
                      }
                  }
              }, {
                  key: "drawClear",
                  value: function () {
                      f(this.context, this.rectMain.left, this.rectMain.top, this.rectMain.width, this.rectMain.height, this.color.back)
                  }
              }, {
                  key: "drawGridline",
                  value: function () {
                      u(this.context, this.color.grid),
                      c(this.context, this.rectMain.left, this.rectMain.top, this.rectMain.width + this.scale / 2, this.rectMain.height),
                      d(this.context)
                  }
              }, {
                  key: "drawButton",
                  value: function () {
                      if ("horizontal" === this.config.direct) {
                          var t = (this.rectChart.height - this.layout.scroll.height) / 2;
                          void 0 !== this.config.txt.head && x(this.context, this.rectChart.left + this.scale, this.rectChart.top + t, this.config.txt.head, this.layout.scroll.font, this.layout.scroll.pixel, this.color.axis),
                          void 0 !== this.config.txt.tail && x(this.context, this.rectChart.left + this.rectChart.width - this.scale, this.rectChart.top + t, this.config.txt.tail, this.layout.scroll.font, this.layout.scroll.pixel, this.color.axis, {
                              x: "end"
                          }),
                          u(this.context, this.color.colume),
                          c(this.context, this.rectMid.left, this.rectMid.top, this.rectMid.width, this.rectMid.height),
                          f(this.context, this.rectMid.left, this.rectMid.top, this.rectMid.width, this.rectMid.height, this.color.box),
                          "free" === this.config.shape && (f(this.context, this.rectMin.left, this.rectMin.top, this.rectMin.width, this.rectMin.height, this.color.colume),
                          f(this.context, this.rectMax.left, this.rectMax.top, this.rectMax.width, this.rectMax.height, this.color.colume)),
                          d(this.context);
                          var i = g(this.context, this.config.txt.left, this.layout.scroll.font, this.layout.scroll.pixel) + 2 * this.scale;
                          void 0 !== this.config.txt.left && this.rectMid.width > i && x(this.context, this.rectMid.left + this.scale, this.rectMid.top + t, this.config.txt.left, this.layout.scroll.font, this.layout.scroll.pixel, this.color.axis),
                          void 0 !== this.config.txt.right && this.rectMid.width > 2 * i && x(this.context, this.rectMid.left + this.rectMid.width - this.scale, this.rectMid.top + t, this.config.txt.right, this.layout.scroll.font, this.layout.scroll.pixel, this.color.axis, {
                              x: "end"
                          })
                      }
                  }
              }]) && dt(i.prototype, e),
              t
          }();
        function xt(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var yt = function () {
            function t(i, e) {
                !function (i, e) {
                    if (!(i instanceof t))
                        throw new TypeError("Cannot call a class as a function")
                }(this),
                F(this, i),
                this.rectMain = e,
                this.linkInfo = i.father.linkInfo,
                this.axisX = i.config.axisX,
                this.maxmin = i.maxmin,
                this.text = i.layout.axisX
            }
            var i, e;
            return i = t,
            (e = [{
                key: "onPaint",
                value: function () {
                    if (this.data = this.father.data,
                    "none" !== this.axisX.display) {
                        var t, i, e;
                        t = this.rectMain.left + this.text.spaceX;
                        var n = this.rectMain.top + this.rectMain.height / 2;
                        if ("block" === this.axisX.display) {
                            var s = -1
                              , o = 0
                              , h = this.linkInfo.maxCount / (this.axisX.lines + 1);
                            e = this.rectMain.width / (this.axisX.lines + 1);
                            for (var r = this.linkInfo.minIndex; r <= this.linkInfo.maxIndex; r++) {
                                var l = Object(T.d)(this.data, "idx", r);
                                l < 0 || s < (o = Math.floor(l / h)) && (s = o,
                                t = this.rectMain.left + e / 2 + e * s,
                                i = Object(a.h)(Object(T.d)(this.data, "time", r)),
                                x(this.context, t, n, i, this.text.font, this.text.pixel, this.color.axis, {
                                    y: "middle",
                                    x: "center"
                                }))
                            }
                        } else
                            i = "tradetime" === this.axisX.format ? (this.tradeTime = this.father.father.dataLayer.tradeTime,
                            i = Object(a.e)(this.data.key, 0, this.tradeTime[0].begin),
                            x(this.context, t, n, i, this.text.font, this.text.pixel, this.color.axis, {
                                y: "middle"
                            }),
                            t = this.rectMain.left + this.rectMain.width - 3,
                            Object(a.e)(this.data.key, 0, this.tradeTime[this.tradeTime.length - 1].end)) : (i = Object(T.d)(this.data, "time", this.linkInfo.minIndex),
                            i = Object(a.e)(this.data.key, i, this.maxmin.min),
                            x(this.context, t, n, i, this.text.font, this.text.pixel, this.color.axis, {
                                y: "middle"
                            }),
                            t = this.rectMain.left + this.rectMain.width - 3,
                            i = Object(T.d)(this.data, "time", this.linkInfo.maxIndex),
                            Object(a.e)(this.data.key, i, this.maxmin.max)),
                            x(this.context, t, n, i, this.text.font, this.text.pixel, this.color.axis, {
                                y: "middle",
                                x: "end"
                            })
                    }
                }
            }]) && xt(i.prototype, e),
            t
        }();
        function gt(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var pt = function () {
            function t(i, e, n) {
                !function (i, e) {
                    if (!(i instanceof t))
                        throw new TypeError("Cannot call a class as a function")
                }(this),
                F(this, i),
                this.rectMain = e,
                this.linkInfo = i.father.linkInfo,
                this.static = i.static,
                this.align = n,
                this.axisY = i.config.axisY,
                this.maxmin = i.maxmin,
                this.text = i.layout.title
            }
            var i, e;
            return i = t,
            (e = [{
                key: "onPaint",
                value: function () {
                    if ("none" !== this.axisY[this.align].display && !this.linkInfo.hideInfo) {
                        var t, i, e, n, s, o = "phone" === this.axisPlatform ? 2 * this.scale : -2 * this.scale;
                        t = "left" === this.align ? "phone" === this.axisPlatform ? (s = "start",
                        this.rectMain.left + o) : (s = "end",
                        this.rectMain.left + this.rectMain.width + o) : "phone" === this.axisPlatform ? (s = "end",
                        this.rectMain.left + this.rectMain.width - o) : (s = "start",
                        this.rectMain.left - o),
                        i = this.rectMain.top + this.scale,
                        "noupper" !== this.axisY[this.align].display && (i = this.rectMain.top + this.scale,
                        n = "before" !== this.axisY[this.align].middle ? this.color.axis : this.color.red,
                        e = Object(a.c)(this.maxmin.max, this.axisY[this.align].format, this.static.coinunit, this.static.volzoom, this.static.before),
                        x(this.context, t, i, e, this.text.font, this.text.pixel, n, {
                            x: s,
                            y: "top"
                        })),
                        "nofoot" !== this.axisY[this.align].display && (n = "before" !== this.axisY[this.align].middle ? this.color.axis : this.color.green,
                        i = this.rectMain.top + this.rectMain.height - this.scale,
                        e = Object(a.c)(this.maxmin.min, this.axisY[this.align].format, this.static.coinunit, this.static.volzoom, this.static.before),
                        x(this.context, t, i, e, this.text.font, this.text.pixel, n, {
                            x: s,
                            y: "bottom"
                        }));
                        for (var h = this.rectMain.height / (this.axisY.lines + 1), r = 0; r < this.axisY.lines; r++)
                            e = this.maxmin.max - h * (r + 1) / this.maxmin.unitY,
                            i = this.rectMain.top + Math.round((r + 1) * h),
                            n = this.color.axis,
                            "none" !== this.axisY[this.align].middle && (r + 1 < Math.round(this.axisY.lines / 2) && (n = this.color.red),
                            r + 1 > Math.round(this.axisY.lines / 2) && (n = this.color.green),
                            r + 1 === Math.round(this.axisY.lines / 2) && (e = "zero" === this.axisY[this.align].middle ? 0 : this.static.before)),
                            e = Object(a.c)(e, this.axisY[this.align].format, this.static.coinunit, this.static.volzoom, this.static.before),
                            x(this.context, t, i, e, this.text.font, this.text.pixel, n, {
                                x: s,
                                y: "middle"
                            })
                    }
                }
            }]) && gt(i.prototype, e),
            t
        }();
        function bt(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var kt = function () {
            function t(i, e, n) {
                !function (i, e) {
                    if (!(i instanceof t))
                        throw new TypeError("Cannot call a class as a function")
                }(this),
                F(this, i),
                this.rectFather = i.rectMain,
                this.rectMain = e,
                this.rectChart = n,
                this.linkInfo = i.father.linkInfo,
                this.static = i.static,
                this.axisXInfo = i.config.axisX,
                this.axisYInfo = i.config.axisY,
                this.maxmin = i.maxmin,
                this.axisX = i.layout.axisX,
                this.context = i.father.cursorCanvas.context
            }
            var i, e;
            return i = t,
            (e = [{
                key: "onClear",
                value: function () {
                    var t, i, e, n, a;
                    t = this.context,
                    i = this.rectFather.left,
                    e = this.rectFather.top,
                    n = this.rectFather.left + this.rectFather.width,
                    a = this.rectFather.top + this.rectFather.height,
                    t.clearRect(i, e, n, a)
                }
            }, {
                key: "onPaint",
                value: function (t, i, e) {
                    if ("function" == typeof this.context._beforePaint && this.context._beforePaint(),
                    !1 !== Object(a.p)(this.rectChart, t.x)) {
                        var n;
                        this.onClear();
                        var s = t.x
                          , o = t.y
                          , h = "phone" === this.axisPlatform ? 2 * this.scale : -2 * this.scale;
                        if (Object(a.q)(this.rectChart, t.y)) {
                            void 0 === e ? e = this.maxmin.max - (t.y - this.rectChart.top) / this.maxmin.unitY : o = (this.maxmin.max - e) * this.maxmin.unitY + this.rectChart.top,
                            I(this.context, this.rectMain.left, o, this.rectMain.left + this.rectMain.width, o, this.color.grid);
                            var r = "phone" === this.axisPlatform ? "start" : "end";
                            "none" !== this.axisYInfo.left.display && (n = Object(a.c)(e, this.axisYInfo.left.format, this.static.coinunit, this.static.volzoom, this.static.before),
                            s = this.rectMain.left + h,
                            b(this.context, s, o, n, {
                                font: this.axisX.font,
                                pixel: this.axisX.pixel,
                                spaceX: this.axisX.spaceX,
                                clr: this.color.txt,
                                bakclr: this.color.grid,
                                x: r,
                                y: "middle"
                            })),
                            "none" !== this.axisYInfo.right.display && (n = Object(a.c)(e, this.axisYInfo.right.format, this.static.coinunit, this.static.volzoom, this.static.before),
                            r = "phone" === this.axisPlatform ? "end" : "start",
                            s = this.rectMain.left + this.rectMain.width - h,
                            b(this.context, s, o, n, {
                                font: this.axisX.font,
                                pixel: this.axisX.pixel,
                                spaceX: this.axisX.spaceX,
                                clr: this.color.txt,
                                bakclr: this.color.grid,
                                x: r,
                                y: "middle"
                            }))
                        }
                        if (I(this.context, t.x, this.rectMain.top, t.x, this.rectMain.top + this.rectMain.height - 1, this.color.grid),
                        "none" !== this.axisXInfo.display) {
                            s = t.x;
                            var l = Math.floor((this.axisX.height - this.axisX.pixel - this.scale) / 2);
                            o = this.rectMain.top + this.rectMain.height + l,
                            n = Object(a.e)(this.father.data.key, i);
                            var c = Math.round(g(this.context, n, this.axisX.font, this.axisX.pixel) / 2)
                              , f = "center";
                            s - c < this.rectMain.left + h && (s = this.rectMain.left + h,
                            f = "start"),
                            s + c > this.rectMain.left + this.rectMain.width - h && (s = this.rectMain.left + this.rectMain.width - h,
                            f = "end"),
                            b(this.context, s, o, n, {
                                font: this.axisX.font,
                                pixel: this.axisX.pixel,
                                spaceX: this.axisX.spaceX,
                                clr: this.color.txt,
                                bakclr: this.color.grid,
                                x: f,
                                y: "top"
                            })
                        }
                        "function" == typeof this.context._afterPaint && this.context._afterPaint()
                    }
                }
            }]) && bt(i.prototype, e),
            t
        }();
        function wt(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var It = function () {
            function t(i, e) {
                !function (i, e) {
                    if (!(i instanceof t))
                        throw new TypeError("Cannot call a class as a function")
                }(this),
                F(this, i),
                this.rectMain = e,
                this.axisX = i.config.axisX,
                this.axisY = i.config.axisY
            }
            var i, e;
            return i = t,
            (e = [{
                key: "onPaint",
                value: function () {
                    if (u(this.context, this.color.grid),
                    h(this.context, this.rectMain.left, this.rectMain.left + this.rectMain.width, this.rectMain.top),
                    0 < this.axisY.lines)
                        for (var t = this.rectMain.height / (this.axisY.lines + 1), i = 0; i < this.axisY.lines; i++)
                            h(this.context, this.rectMain.left, this.rectMain.left + this.rectMain.width, this.rectMain.top + Math.round((i + 1) * t));
                    if ("none" !== this.axisX.display && h(this.context, this.rectMain.left, this.rectMain.left + this.rectMain.width, this.rectMain.top + this.rectMain.height),
                    0 < this.axisX.lines)
                        for (var e = this.rectMain.width / (this.axisX.lines + 1), n = 0; n < this.axisX.lines; n++)
                            o(this.context, this.rectMain.left + Math.round((n + 1) * e), this.rectMain.top, this.rectMain.top + this.rectMain.height);
                    "phone" !== this.axisPlatform && (o(this.context, this.rectMain.left, this.rectMain.top, this.rectMain.top + this.rectMain.height),
                    o(this.context, this.rectMain.left + this.rectMain.width, this.rectMain.top, this.rectMain.top + this.rectMain.height)),
                    d(this.context)
                }
            }]) && wt(i.prototype, e),
            t
        }();
        function Mt(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var Ot = function () {
            function t(i, e, n) {
                !function (i, e) {
                    if (!(i instanceof t))
                        throw new TypeError("Cannot call a class as a function")
                }(this),
                F(this, i),
                this.rectMain = e,
                this.rectMess = n,
                this.linkInfo = i.father.linkInfo,
                this.title = i.layout.title,
                this.titleInfo = i.config.title
            }
            var i, e;
            return i = t,
            (e = [{
                key: "onPaint",
                value: function (t) {
                    if ("none" !== this.titleInfo.display && !this.linkInfo.hideInfo) {
                        f(this.context, this.rectMain.left + this.scale, this.rectMain.top + this.scale, this.rectMess.left + this.rectMess.width - 2 * this.scale, this.rectMain.height - 2 * this.scale, this.color.back);
                        var i = this.color.txt
                          , e = Math.round((this.title.height - this.title.pixel) / 2) - this.scale
                          , n = this.rectMess.top + e;
                        void 0 !== this.titleInfo.label && x(this.context, this.rectMain.left + this.scale, n, this.titleInfo.label, this.title.font, this.title.pixel, i);
                        for (var a = this.rectMess.left + this.scale, s = 0; s < t.length; s++)
                            i = this.color.line[s],
                            void 0 !== t[s].txt && (x(this.context, a, n, t[s].txt, this.title.font, this.title.pixel, i),
                            a += g(this.context, t[s].txt, this.title.font, this.title.pixel) + this.title.spaceX),
                            void 0 !== t[s].value && (x(this.context, a, n, " " + t[s].value, this.title.font, this.title.pixel, i),
                            a += g(this.context, " " + t[s].value, this.title.font, this.title.pixel) + this.title.spaceX)
                    }
                }
            }]) && Mt(i.prototype, e),
            t
        }();
        function Dt(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var Ct = function () {
            function t(i) {
                !function (t, i) {
                    if (!(t instanceof i))
                        throw new TypeError("Cannot call a class as a function")
                }(this, t),
                F(this, i),
                this.linkInfo = i.linkInfo,
                this.static = this.father.dataLayer.static,
                this.data = {},
                this.maxmin = {}
            }
            var i, e;
            return i = t,
            (e = [{
                key: "init",
                value: function (t, i) {
                    this.callback = i,
                    this.rectMain = t.rectMain || {
                        left: 0,
                        top: 0,
                        width: 400,
                        height: 200
                    },
                    this.layout = Object(a.v)(t.layout, $),
                    this.config = Object(a.b)(t.config),
                    this.buttons = t.buttons || [],
                    this.scroll = t.config.scroll || {
                        display: "none"
                    },
                    this.childCharts = {},
                    this.checkConfig(),
                    this.setPublicRect(),
                    this.childDraws = {},
                    this.setChildDraw(),
                    this.childLines = {},
                    this.setChildLines(),
                    this.setButtons(),
                    this.setScroll()
                }
            }, {
                key: "checkConfig",
                value: function () {
                    R(this.layout),
                    void 0 !== this.config.zoomInfo && this.setZoomInfo()
                }
            }, {
                key: "setPublicRect",
                value: function () {
                    this.rectChart = Object(a.u)(this.rectMain, this.layout.margin);
                    var t = {
                        width: this.layout.axisX.width
                    };
                    this.rectTitle = {
                        left: this.rectMain.left,
                        top: this.rectMain.top,
                        width: 0,
                        height: 0
                    },
                    this.rectMess = {
                        left: this.rectMain.left,
                        top: this.rectMain.top,
                        width: 0,
                        height: 0
                    },
                    "none" !== this.config.title.display && (this.rectTitle = {
                        left: this.rectChart.left,
                        top: this.rectChart.top,
                        width: t.width,
                        height: this.layout.title.height
                    },
                    this.rectMess = {
                        left: this.rectChart.left + t.width + this.scale,
                        top: this.rectChart.top,
                        width: this.rectChart.width - 2 * t.width,
                        height: this.layout.title.height
                    }),
                    t.left = this.rectChart.left,
                    t.right = this.rectChart.left + this.rectChart.width,
                    t.top = this.rectTitle.top + this.rectTitle.height,
                    t.bottom = this.rectChart.top + this.rectChart.height,
                    "phone" !== this.axisPlatform && ("none" !== this.config.axisY.left.display && (t.left += this.layout.axisX.width),
                    "none" !== this.config.axisY.right.display && (t.right -= this.layout.axisX.width)),
                    "none" !== this.config.axisX.display && (t.bottom -= this.layout.axisX.height),
                    "none" !== this.scroll.display && (t.bottom -= this.layout.scroll.size),
                    this.rectGridLine = {
                        left: t.left,
                        top: t.top,
                        width: t.right - t.left,
                        height: t.bottom - t.top
                    },
                    this.rectAxisYLeft = {
                        left: this.rectChart.left,
                        top: t.top,
                        width: t.width,
                        height: t.bottom - t.top
                    },
                    this.rectAxisYRight = {
                        left: this.rectChart.left + this.rectChart.width - t.width,
                        top: t.top,
                        width: t.width,
                        height: t.bottom - t.top
                    },
                    this.rectChart = Object(a.u)(this.rectGridLine, this.layout.offset),
                    this.rectAxisX = {
                        left: 0,
                        top: t.bottom,
                        width: 0,
                        height: 0
                    },
                    "none" !== this.config.axisX.display && (this.rectAxisX = {
                        left: t.left,
                        top: t.bottom + this.scale,
                        width: t.right - t.left,
                        height: this.layout.axisX.height
                    }),
                    "none" !== this.scroll.display && (this.rectScroll = {
                        left: t.left,
                        top: this.rectAxisX.top + this.rectAxisX.height + this.scale,
                        width: t.right - t.left,
                        height: this.layout.scroll.size
                    })
                }
            }, {
                key: "getLineDataKey",
                value: function (t) {
                    return void 0 === t.formula ? this.hotKey : t.formula.key
                }
            }, {
                key: "setChildLines",
                value: function () {
                    for (var t, i = 0, e = 0; e < this.config.lines.length; e++) {
                        var n = this.config.lines[e].className;
                        t = new n(this, this.rectChart),
                        (this.childLines["NAME" + e] = t).name = "NAME" + e,
                        t.hotKey = this.getLineDataKey(this.config.lines[e]),
                        Object(a.o)(n, [W, J]) && (t.info = {
                            labelX: "idx",
                            labelY: "value"
                        },
                        void 0 !== this.config.lines[e].info && (t.info = this.config.lines[e].info),
                        "day5" === this.config.axisX.type && (t.info.skips = Object(T.j)(this.father.dataLayer.tradeTime)),
                        t.info.colorIndex = i,
                        i++)
                    }
                }
            }, {
                key: "setChildDraw",
                value: function () {
                    var t;
                    t = new kt(this, this.rectGridLine, this.rectChart),
                    this.childDraws.CURSOR = t,
                    "none" !== this.config.title.display && (t = new Ot(this, this.rectTitle, this.rectMess),
                    this.childDraws.TITLE = t),
                    "none" !== this.config.axisY.left.display && (t = new pt(this, this.rectAxisYLeft, "left"),
                    this.childDraws.AXISY_LEFT = t),
                    "none" !== this.config.axisY.right.display && (t = new pt(this, this.rectAxisYRight, "right"),
                    this.childDraws.AXISY_RIGHT = t),
                    "none" !== this.config.axisX.display && (t = new yt(this, this.rectAxisX),
                    this.childDraws.AXISX = t),
                    t = new It(this, this.rectGridLine),
                    this.childDraws.GRID = t
                }
            }, {
                key: "setScroll",
                value: function () {
                    if ("none" !== this.scroll.display) {
                        var t = new mt(this);
                        t.name = "HSCROLL",
                        (this.childCharts[t.name] = t).init({
                            rectMain: this.rectScroll,
                            config: {
                                width: 8
                            }
                        }, function (t) {
                            var i = t.self.father;
                            i.father.linkInfo.minIndex !== t.minIndex && (i.father.linkInfo.minIndex = t.minIndex,
                            i.father.onPaint())
                        })
                    }
                }
            }, {
                key: "createButton",
                value: function (t) {
                    if (void 0 !== this.childCharts[t])
                        return this.childCharts[t];
                    var i = new ut(this);
                    return i.name = t,
                    this.childCharts[t] = i
                }
            }, {
                key: "hasButton",
                value: function (t, i) {
                    for (var e = 0; e < i.length; e++)
                        if (t === i[e].key)
                            return !0;
                    return !1
                }
            }, {
                key: "setButtons",
                value: function () {
                    var t, i, e, n = this, a = 25 * this.scale;
                    (this.hasButton("zoomin", this.buttons) || this.hasButton("zoomout", this.buttons)) && (t = this.createButton("zoomin"),
                    i = Math.floor((this.rectChart.width - 2 * (a + a)) / 4),
                    e = this.rectChart.top + .95 * this.rectChart.height - a,
                    t.init({
                        rectMain: {
                            left: i,
                            top: e,
                            width: a,
                            height: a
                        },
                        info: [{
                            map: "-"
                        }]
                    }, function () {
                        0 < n.config.zoomInfo.index && (n.config.zoomInfo.index--,
                        n.setZoomInfo(),
                        n.father.onPaint())
                    }),
                    i += 2 * a,
                    (t = this.createButton("zoomout")).init({
                        rectMain: {
                            left: i,
                            top: e,
                            width: a,
                            height: a
                        },
                        info: [{
                            map: "+"
                        }]
                    }, function () {
                        n.config.zoomInfo.index < n.config.zoomInfo.list.length - 1 && (n.config.zoomInfo.index++,
                        n.setZoomInfo(),
                        n.father.onPaint())
                    })),
                    this.hasButton("exright", this.buttons) && (t = this.createButton("exright"),
                    a = g(this.context, "前复权", this.layout.title.font, this.layout.title.pixel),
                    i = this.rectMain.left + this.rectMain.width - a - this.layout.title.spaceX,
                    e = "none" !== this.config.title.display ? (this.rectMess.height - this.layout.title.pixel - 2 * this.layout.title.spaceY) / 2 : this.rectMain.top + this.layout.title.spaceY,
                    t.init({
                        rectMain: {
                            left: i,
                            top: this.rectMess.top + e,
                            width: a + this.layout.title.spaceX,
                            height: this.layout.title.pixel + 2 * this.layout.title.spaceY
                        },
                        config: {
                            shape: "box"
                        },
                        info: [{
                            map: "不除权",
                            value: "no"
                        }, {
                            map: "前复权",
                            value: "forword"
                        }]
                    }, function (t) {
                        n.father.linkInfo.rightMode = t.info.value,
                        n.father.fastDrawEnd(),
                        n.father.onPaint()
                    }),
                    t.hotIdx = 0)
                }
            }, {
                key: "addLine",
                value: function (t) {
                    this.config.lines.push(t)
                }
            }, {
                key: "removeLine",
                value: function (t) {
                    for (var i = 0; i < this.config.lines.length; i++)
                        void 0 !== this.config.lines[i].name && this.config.lines[i].name === t && this.config.lines.splice(i, 1)
                }
            }, {
                key: "setZoomInfo",
                value: function () {
                    var t = this.config.zoomInfo;
                    t.index = t.index > t.list.length - 1 ? t.list.length - 1 : t.index,
                    t.index = t.index < 0 ? 0 : t.index;
                    var i = t.list[t.index];
                    this.linkInfo.unitX = i * this.scale,
                    this.linkInfo.unitX < this.scale && (this.linkInfo.unitX = this.scale),
                    this.linkInfo.spaceX = this.linkInfo.unitX / 4,
                    this.linkInfo.spaceX < this.scale && (this.linkInfo.spaceX = this.scale),
                    this.childCharts.zoomin && (0 === t.index ? this.childCharts.zoomin.setStatus("disabled") : this.childCharts.zoomin.setStatus("enabled")),
                    this.childCharts.zoomout && (t.index === t.list.length - 1 ? this.childCharts.zoomout.setStatus("disabled") : this.childCharts.zoomout.setStatus("enabled")),
                    this.father.fastDrawEnd()
                }
            }, {
                key: "setHotButton",
                value: function (t) {
                    for (var i in this.childCharts)
                        this.childCharts[i] === t ? this.childCharts[i].focused = !0 : this.childCharts[i].focused = !1
                }
            }, {
                key: "drawClear",
                value: function () {
                    f(this.context, this.rectMain.left, this.rectMain.top, this.rectMain.width, this.rectMain.height, this.color.back),
                    u(this.context, this.color.grid),
                    c(this.context, this.rectMain.left, this.rectMain.top, this.rectMain.width, this.rectMain.height),
                    d(this.context)
                }
            }, {
                key: "drawChildCharts",
                value: function () {
                    var t;
                    for (var i in this.childCharts)
                        this.childCharts[i].focused ? t = this.childCharts[i] : this.childCharts[i].onPaint();
                    t && t.onPaint()
                }
            }, {
                key: "beforeLocation",
                value: function () {
                    for (var t in this.childLines)
                        this.childLines[t].beforeLocation && this.childLines[t].beforeLocation()
                }
            }, {
                key: "drawChildLines",
                value: function () {
                    for (var t in this.childLines)
                        void 0 !== this.childLines[t].hotKey ? this.childLines[t].onPaint(this.childLines[t].hotKey) : this.childLines[t].onPaint(this.hotKey)
                }
            }, {
                key: "getMoveData",
                value: function (t) {
                    var i, e, n = this.config.lines, s = [];
                    if (!Array.isArray(n))
                        return s;
                    for (var o = 0; o < n.length; o++)
                        void 0 !== n[o].info && (void 0 !== n[o].info.labelY ? (i = void 0 === n[o].formula ? Object(T.d)(this.data, n[o].info.labelY, t) : Object(T.d)(this.father.getData(n[o].formula.key), n[o].info.labelY, t - this.linkInfo.minIndex),
                        e = Object(a.c)(i, n[o].info.format, this.static.coinunit, this.static.volzoom),
                        s.push({
                            index: o,
                            txt: n[o].info.txt,
                            value: e
                        })) : s.push({
                            index: o,
                            txt: n[o].info.txt
                        }));
                    return s
                }
            }, {
                key: "drawTitleInfo",
                value: function (t) {
                    "none" !== this.config.title.display && ((void 0 === t || t < 0 || t > this.linkInfo.maxIndex) && (t = this.linkInfo.maxIndex),
                    this.childDraws.TITLE.onPaint(this.getMoveData(t)))
                }
            }, {
                key: "drawChildDraw",
                value: function (t) {
                    void 0 !== this.childDraws[t] && ("TITLE" === t ? this.drawTitleInfo(this.linkInfo.moveIndex) : this.childDraws[t].onPaint())
                }
            }, {
                key: "onPaint",
                value: function () {
                    this.beforeLocation(),
                    this.data = this.father.getData(this.hotKey),
                    this.locationData(),
                    this.father.readyData(this.data, this.config.lines),
                    s(this.context, this.scale),
                    this.drawClear(),
                    this.drawChildDraw("GRID"),
                    this.readyDraw(),
                    this.drawChildDraw("AXISX"),
                    this.drawChildDraw("AXISY_LEFT"),
                    this.drawChildDraw("AXISY_RIGHT"),
                    this.drawChildLines(),
                    this.drawChildDraw("TITLE"),
                    this.drawChildCharts()
                }
            }, {
                key: "getMiddle",
                value: function (t) {
                    var i = this.config.axisY.left.middle;
                    return "fixedRight" === t && (i = this.config.axisY.right.middle),
                    "before" === i ? this.static.before : 0
                }
            }, {
                key: "calcMaxMin",
                value: function (t, i, e, n) {
                    var s, o = {
                        max: 0,
                        min: 0
                    };
                    if ("fixedLeft" === i.method || "fixedRight" === i.method) {
                        var h = this.getMiddle(i.method);
                        o.min = .99 * h,
                        o.max = 1.01 * h
                    }
                    if (void 0 === t || Object(a.t)(t.value))
                        return o;
                    void 0 === e && (e = 0),
                    void 0 === n && (n = t.value.length - 1);
                    for (var r = e; r <= n; r++) {
                        if (!Object(a.t)(i.maxvalue))
                            for (var l = 0; l < i.maxvalue.length; l++)
                                "string" == typeof i.maxvalue[l] && 0 < (s = Object(T.d)(t, i.maxvalue[l], r)) && s > o.max && (o.max = s);
                        if (!Object(a.t)(i.minvalue))
                            for (var c = 0; c < i.minvalue.length; c++)
                                "string" == typeof i.minvalue[c] && (s = Object(T.d)(t, i.minvalue[c], r),
                                0 === o.min && (o.min = s),
                                0 < s && s < o.min && (o.min = s))
                    }
                    if (!Object(a.t)(i.maxvalue))
                        for (var f = 0; f < i.maxvalue.length; f++)
                            if ("number" == typeof i.maxvalue[f]) {
                                o.max = i.maxvalue[f];
                                break
                            }
                    if (!Object(a.t)(i.minvalue))
                        for (var u = 0; u < i.minvalue.length; u++)
                            if ("number" == typeof i.minvalue[u]) {
                                o.min = i.minvalue[u];
                                break
                            }
                    if ("fixedLeft" === i.method || "fixedRight" === i.method) {
                        var d = this.getMiddle(i.method);
                        o.max === o.min && (o.max > d && (o.min = d),
                        o.min < d && (o.max = d));
                        var v = Math.abs(o.max - d) / d
                          , m = Math.abs(d - o.min) / d;
                        v < .01 && m < .01 && this.static.stktype !== lt.STOCK_TYPE_INDEX ? (o.min = .99 * d,
                        o.max = 1.01 * d) : m < v ? o.min = d * (1 - v) : o.max = d * (1 + m),
                        o.min < 0 && (o.min = 0)
                    }
                    return o
                }
            }, {
                key: "readyScroll",
                value: function () {
                    if ("none" !== this.scroll.display && void 0 !== this.childCharts.HSCROLL) {
                        var t = Object(T.d)(this.data, "time", this.linkInfo.minIndex);
                        t = Object(a.e)(this.data.key, t, this.father.dataLayer.tradeTime[0].begin);
                        var i = Object(T.d)(this.data, "time", this.linkInfo.maxIndex);
                        i = Object(a.e)(this.data.key, i, this.father.dataLayer.tradeTime[this.father.dataLayer.tradeTime.length - 1].end);
                        var e = Object(T.d)(this.data, "time", 0);
                        e = Object(a.e)(this.data.key, e, this.father.dataLayer.tradeTime[0].begin);
                        var n = Object(T.d)(this.data, "time", this.data.value.length - 1);
                        n = Object(a.e)(this.data.key, n, this.father.dataLayer.tradeTime[this.father.dataLayer.tradeTime.length - 1].end),
                        this.childCharts.HSCROLL.onChange({
                            head: e,
                            tail: n,
                            left: t,
                            right: i,
                            min: this.linkInfo.minIndex,
                            max: this.linkInfo.maxIndex,
                            range: this.data.value.length
                        })
                    }
                }
            }, {
                key: "getDataRange",
                value: function (t) {
                    var i = {
                        minIndex: -1,
                        maxIndex: -1
                    };
                    if (Object(a.t)(t.value) || Object(a.t)(this.data.value))
                        return i;
                    for (var e = Object(T.d)(this.data, "time", this.linkInfo.minIndex), n = Object(T.d)(this.data, "time", this.linkInfo.maxIndex), s = 0; s <= t.value.length - 1; s++)
                        if (e <= Object(T.d)(t, "time", s)) {
                            i.minIndex = s;
                            break
                        }
                    for (var o = t.value.length - 1; 0 <= o; o--)
                        if (Object(T.d)(t, "time", o) <= n) {
                            i.maxIndex = o;
                            break
                        }
                    return i
                }
            }, {
                key: "locationData",
                value: function () {
                    if (void 0 !== this.data) {
                        var t = Object(T.k)(this.data);
                        "day1" === this.config.axisX.type ? Y(this.linkInfo, {
                            width: this.rectChart.width,
                            size: t,
                            scale: this.scale,
                            maxCount: Object(T.j)(this.father.dataLayer.tradeTime)
                        }) : "day5" === this.config.axisX.type ? Y(this.linkInfo, {
                            width: this.rectChart.width,
                            size: t,
                            scale: this.scale,
                            maxCount: 5 * Object(T.j)(this.father.dataLayer.tradeTime)
                        }) : function (t, i) {
                            var e = t.unitX / 4;
                            t.spaceX = e < i.scale ? i.scale : e,
                            t.maxCount = Math.floor(i.width / (t.unitX + t.spaceX));
                            var n = t.maxCount > i.size ? i.size : t.maxCount;
                            switch (t.showMode) {
                                case "fixed":
                                    t.maxIndex = i.size - 1,
                                    t.minIndex = i.size - n;
                                    break;
                                case "locked":
                                case "move":
                                    break;
                                default:
                                    t.maxIndex = i.size - 1,
                                    t.minIndex = i.size - n
                            }
                        }(this.linkInfo, {
                            width: this.rectChart.width,
                            scale: this.scale,
                            size: t
                        })
                    }
                }
            }, {
                key: "readyDraw",
                value: function () {
                    var t, i;
                    this.readyScroll();
                    for (var e = 0; e < this.config.lines.length; e++)
                        if (!(void 0 === this.config.lines[e].extremum || Object(a.t)(this.config.lines[e].extremum.maxvalue) && Object(a.t)(this.config.lines[e].extremum.minvalue))) {
                            var n = this.config.lines[e].formula;
                            if (void 0 !== n) {
                                var s = this.father.getData(n.key)
                                  , o = this.getDataRange(s);
                                t = this.calcMaxMin(s, this.config.lines[e].extremum, o.minIndex, o.maxIndex)
                            } else
                                t = this.calcMaxMin(this.data, this.config.lines[e].extremum, this.linkInfo.minIndex, this.linkInfo.maxIndex);
                            void 0 === i ? i = t : (i.max = i.max > t.max ? i.max : t.max,
                            i.min = i.min < t.min ? i.min : t.min)
                        }
                    this.maxmin.max = i.max,
                    this.maxmin.min = i.min,
                    this.maxmin.unitY = (this.rectChart.height - 2) / (this.maxmin.max - this.maxmin.min)
                }
            }, {
                key: "onClick",
                value: function (t) {
                    "phone1" !== this.axisPlatform && (this.linkInfo.showCursorLine = !this.linkInfo.showCursorLine,
                    this.linkInfo.showCursorLine ? this.father.eventLayer.boardEvent(this.father, "onMouseMove", t) : (t.reDraw = !0,
                    this.father.eventLayer.boardEvent(this.father, "onMouseOut", t)))
                }
            }, {
                key: "onLongPress",
                value: function (t) {
                    this.linkInfo.showCursorLine = !0,
                    this.father.eventLayer.boardEvent(this.father, "onMouseMove", t)
                }
            }, {
                key: "onPinch",
                value: function (t) {
                    this.config.zoomInfo && (0 < t.scale ? this.config.zoomInfo.index++ : this.config.zoomInfo.index--,
                    this.config.zoomInfo.index < 0 ? this.config.zoomInfo.index = 0 : (this.setZoomInfo(2 * this.config.zoomInfo.index + 1),
                    this.father.onPaint()))
                }
            }, {
                key: "onMouseOut",
                value: function (t) {
                    (this.linkInfo.showCursorLine || t.reDraw) && (this.linkInfo.moveIndex = this.linkInfo.maxIndex,
                    this.drawTitleInfo(this.linkInfo.moveIndex)),
                    this.childDraws.CURSOR.onClear()
                }
            }, {
                key: "onMouseWheel",
                value: function (t) {
                    if (void 0 !== this.config.zoomInfo) {
                        var i, e = Math.floor(t.deltaY / 10);
                        0 === e && (e = 0 < t.deltaY ? 1 : -1),
                        0 <= (i = 0 < e ? this.config.zoomInfo.index - 1 : this.config.zoomInfo.index + 1) && i <= this.config.zoomInfo.list.length - 1 && (this.config.zoomInfo.index = i,
                        this.setZoomInfo(),
                        this.father.onPaint())
                    }
                }
            }, {
                key: "onKeyDown",
                value: function (t) {
                    t.keyCode
                }
            }, {
                key: "onMouseMove",
                value: function (t) {
                    if (!this.linkInfo.hideInfo && this.linkInfo.showCursorLine) {
                        var i, e = t.mousePos, n = this.getMouseMoveData(e.x), s = n;
                        0 < n && (i = "day1" === this.config.axisX.type || "day5" === this.config.axisX.type ? (s %= 240,
                        s = Object(T.g)(s, this.father.dataLayer.tradeTime, this.father.dataLayer.tradeDate),
                        function (t, i, e) {
                            for (var n = 0; n <= t.value.length - 1; n++)
                                if (i === Object(T.d)(t, "idx", n))
                                    return n;
                            return -1
                        }(this.data, n)) : (s = Object(T.d)(this.data, "time", n),
                        n),
                        Object(a.p)(this.rectChart, e.x) && this.drawTitleInfo(i),
                        this.linkInfo.moveIndex !== n && (this.linkInfo.moveIndex = n,
                        this.callback({
                            event: "mousemove",
                            before: 0 < n ? Object(T.d)(this.data, "close", n - 1) : Object(T.d)(this.data, "open", 0),
                            data: this.data.value[n]
                        }))),
                        this.childDraws.CURSOR.onPaint(e, s, void 0)
                    }
                }
            }, {
                key: "getMouseMoveData",
                value: function (t) {
                    var i = Math.round((t - this.rectChart.left) / (this.linkInfo.unitX + this.linkInfo.spaceX) - .5);
                    return "day1" === this.config.axisX.type ? i : "day5" === this.config.axisX.type ? i : this.linkInfo.minIndex + i
                }
            }]) && Dt(i.prototype, e),
            t
        }();
        function jt(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var Tt = function () {
            function t(i) {
                !function (t, i) {
                    if (!(t instanceof i))
                        throw new TypeError("Cannot call a class as a function")
                }(this, t),
                F(this, i),
                this.linkInfo = i.linkInfo,
                this.static = this.father.dataLayer.static
            }
            var i, e;
            return i = t,
            (e = [{
                key: "init",
                value: function (t) {
                    this.rectMain = t.rectMain || {
                        left: 0,
                        top: 0,
                        width: 200,
                        height: 300
                    },
                    this.layout = Object(a.v)(t.layout, $),
                    this.config = Object(a.v)(t.config, tt),
                    this.style = t.config.style || "normal",
                    this.checkConfig(),
                    this.setPublicRect()
                }
            }, {
                key: "checkConfig",
                value: function () {
                    R(this.layout),
                    this.txtLen = g(this.context, "涨", this.layout.digit.font, this.layout.digit.pixel),
                    this.timeLen = g(this.context, "15:30", this.layout.digit.font, this.layout.digit.pixel),
                    this.volLen = g(this.context, "888888", this.layout.digit.font, this.layout.digit.pixel),
                    this.closeLen = g(this.context, "888.88", this.layout.digit.font, this.layout.digit.pixel)
                }
            }, {
                key: "setPublicRect",
                value: function () {
                    this.rectChart = Object(a.u)(this.rectMain, this.layout.margin)
                }
            }, {
                key: "onClick",
                value: function () {
                    this.isIndex || ("normal" === this.style ? this.style = "tiny" : this.style = "normal",
                    this.father.onPaint(this))
                }
            }, {
                key: "onPaint",
                value: function () {
                    this.codeInfo = this.father.getData("INFO"),
                    this.orderData = this.father.getData("NOW"),
                    this.tickData = this.father.getData("TICK"),
                    void 0 !== this.orderData && void 0 !== this.tickData && (this.orderData.coinunit = this.static.coinunit,
                    this.tickData.coinunit = this.static.coinunit,
                    this.isIndex = 0 === Object(T.d)(this.codeInfo, "type"),
                    s(this.context, this.scale),
                    this.drawClear(),
                    this.drawReady(),
                    this.isIndex ? this.drawIndex() : this.drawOrder(),
                    this.drawTick())
                }
            }, {
                key: "drawClear",
                value: function () {
                    f(this.context, this.rectMain.left, this.rectMain.top, this.rectMain.width, this.rectMain.height, this.color.back)
                }
            }, {
                key: "drawReady",
                value: function () {
                    var t;
                    void 0 === this.tickData && (this.tickData = {
                        key: "TICK",
                        fields: lt.FIELD_TICK,
                        value: []
                    }),
                    void 0 === this.orderData && (this.orderData = {
                        key: "NOW",
                        fields: lt.FIELD_NOW,
                        value: []
                    }),
                    t = "normal" === this.style ? this.rectChart.top + 10 * (this.layout.digit.height + this.layout.digit.spaceX) : this.rectChart.top + 2 * (this.layout.digit.height + this.layout.digit.spaceX),
                    this.isIndex && (t = this.rectChart.top + 4 * (this.layout.digit.height + this.layout.digit.spaceX)),
                    this.rectOrder = {
                        left: this.rectChart.left,
                        top: this.rectChart.top,
                        width: this.rectChart.width,
                        height: t
                    },
                    "none" !== this.config.title.display ? this.rectTitle = {
                        left: this.rectChart.left,
                        top: t,
                        width: this.rectChart.width,
                        height: this.layout.title.height
                    } : this.rectTitle = {
                        left: 0,
                        top: 0,
                        width: 0,
                        height: 0
                    },
                    t += this.rectTitle.height,
                    this.rectTick = {
                        left: this.rectChart.left,
                        top: t,
                        width: this.rectChart.width,
                        height: this.rectChart.height - t - this.layout.digit.height / 2
                    }
                }
            }, {
                key: "getColor",
                value: function (t, i) {
                    return i < t ? this.color.red : t < i ? this.color.green : this.color.white
                }
            }, {
                key: "drawIndex",
                value: function () {
                    u(this.context, this.color.grid),
                    c(this.context, this.rectMain.left, this.rectMain.top, this.rectMain.width, this.rectMain.height);
                    var t, i, e, n, s = this.rectOrder.height / 3, o = this.rectOrder.width / 3;
                    i = this.rectOrder.top + Math.floor((s - this.layout.digit.height) / 2),
                    t = this.rectOrder.left + (o - this.txtLen) / 2,
                    x(this.context, t, i, "涨", this.layout.digit.font, this.layout.digit.pixel, this.color.txt),
                    t = this.rectOrder.left + o + (o - this.txtLen) / 2,
                    x(this.context, t, i, "跌", this.layout.digit.font, this.layout.digit.pixel, this.color.txt),
                    t = this.rectOrder.left + 2 * o + (o - this.txtLen) / 2,
                    x(this.context, t, i, "平", this.layout.digit.font, this.layout.digit.pixel, this.color.txt);
                    var r = {
                        key: "NOW",
                        fields: lt.FIELD_NOW_IDX,
                        value: this.orderData.value
                    };
                    if (i = this.rectOrder.top + s + Math.floor((s - this.layout.digit.height) / 2),
                    n = Object(a.f)(Object(T.d)(r, "ups"), 1),
                    e = g(this.context, n, this.layout.digit.font, this.layout.digit.pixel),
                    t = this.rectOrder.left + (o - e) / 2,
                    x(this.context, t, i, n, this.layout.digit.font, this.layout.digit.pixel, this.color.txt),
                    n = Object(a.f)(Object(T.d)(r, "downs"), 1),
                    e = g(this.context, n, this.layout.digit.font, this.layout.digit.pixel),
                    t = this.rectOrder.left + o + (o - e) / 2,
                    x(this.context, t, i, n, this.layout.digit.font, this.layout.digit.pixel, this.color.txt),
                    n = Object(a.f)(Object(T.d)(r, "mids"), 1),
                    e = g(this.context, n, this.layout.digit.font, this.layout.digit.pixel),
                    t = this.rectOrder.left + 2 * o + (o - e) / 2,
                    x(this.context, t, i, n, this.layout.digit.font, this.layout.digit.pixel, this.color.txt),
                    i = this.rectOrder.top + 2 * s + Math.floor((s - this.layout.digit.height) / 2),
                    n = Object(a.f)(Object(T.d)(r, "upvol"), 1),
                    e = g(this.context, n, this.layout.digit.font, this.layout.digit.pixel),
                    t = this.rectOrder.left + (o - e) / 2,
                    x(this.context, t, i, n, this.layout.digit.font, this.layout.digit.pixel, this.color.txt),
                    n = Object(a.f)(Object(T.d)(r, "downvol"), 1),
                    e = g(this.context, n, this.layout.digit.font, this.layout.digit.pixel),
                    t = this.rectOrder.left + o + (o - e) / 2,
                    x(this.context, t, i, n, this.layout.digit.font, this.layout.digit.pixel, this.color.txt),
                    "none" !== this.config.title.display) {
                        h(this.context, this.rectTitle.left, this.rectTitle.left + this.rectTitle.width, this.rectTitle.top),
                        h(this.context, this.rectTitle.left, this.rectTitle.left + this.rectTitle.width, this.rectTitle.top + this.rectTitle.height);
                        var l = g(this.context, "分时成交", this.layout.title.font, this.layout.digit.pixel);
                        t = this.rectTitle.left + (this.rectTitle.width - l) / 2,
                        i = this.rectTitle.top + 3 * this.scale,
                        x(this.context, t, i, "分时成交", this.layout.digit.font, this.layout.digit.pixel, this.color.txt)
                    }
                    d(this.context)
                }
            }, {
                key: "drawOrder",
                value: function () {
                    var t = this.drawGridLine();
                    if (!(void 0 === this.orderData || this.orderData.value.length < 1)) {
                        var i = (this.rectOrder.width - t - 2 * this.layout.digit.spaceX - this.closeLen - this.volLen) / 2
                          , e = 1;
                        "normal" === this.style && (e = 5);
                        var n, s, o, h, r = this.rectOrder.height / (2 * e);
                        s = this.rectOrder.top + Math.floor((r - this.layout.digit.height) / 2);
                        for (var l = e; 1 <= l; l--)
                            n = this.rectOrder.left + t + i + this.closeLen,
                            this.linkInfo.hideInfo || (o = Object(T.d)(this.orderData, "sell" + l),
                            h = this.getColor(o, this.static.before),
                            x(this.context, n, s, Object(a.d)(o, this.static.coinunit), this.layout.digit.font, this.layout.digit.pixel, h, {
                                x: "end"
                            })),
                            n += i + this.volLen + this.layout.digit.spaceX,
                            o = Object(T.d)(this.orderData, "sellvol" + l),
                            h = this.color.vol,
                            x(this.context, n, s, Object(a.f)(o, this.static.volzoom), this.layout.digit.font, this.layout.digit.pixel, h, {
                                x: "end"
                            }),
                            s += r;
                        for (var c = 1; c <= e; c++)
                            n = this.rectOrder.left + t + i + this.closeLen,
                            this.linkInfo.hideInfo || (o = Object(T.d)(this.orderData, "buy" + c),
                            h = this.getColor(o, this.static.before),
                            x(this.context, n, s, Object(a.d)(o, this.static.coinunit), this.layout.digit.font, this.layout.digit.pixel, h, {
                                x: "end"
                            })),
                            n += i + this.volLen + this.layout.digit.spaceX,
                            o = Object(T.d)(this.orderData, "buyvol" + c),
                            h = this.color.vol,
                            x(this.context, n, s, Object(a.f)(o, this.static.volzoom), this.layout.digit.font, this.layout.digit.pixel, h, {
                                x: "end"
                            }),
                            s += r
                    }
                }
            }, {
                key: "drawTick",
                value: function () {
                    if (!(void 0 === this.tickData || this.tickData.value.length < 1)) {
                        var t, i, e, n, s = Math.floor(this.rectTick.height / this.layout.digit.height) - 1, o = this.tickData.value.length, h = s < o ? o - s : 0, r = this.rectTick.height / s, l = (this.rectTick.width - 4 * this.layout.digit.spaceX - this.timeLen - this.closeLen - this.volLen) / 2;
                        this.isIndex && (l = (this.rectTick.width - 3 * this.layout.digit.spaceX - this.timeLen - this.closeLen) / 2),
                        i = this.rectTick.top + 2 + Math.floor((r - this.layout.digit.pixel) / 2);
                        for (var c = o - 1; h <= c; c--) {
                            t = this.rectTick.left + this.layout.digit.spaceX + this.timeLen,
                            e = Object(T.d)(this.tickData, "time", c),
                            n = this.color.txt;
                            var f;
                            f = 0 === c ? Object(a.g)(e, "minute") : Object(a.g)(e, "minute", Object(T.d)(this.tickData, "time", c - 1)),
                            x(this.context, t, i, f, this.layout.digit.font, this.layout.digit.pixel, n, {
                                x: "end"
                            }),
                            this.isIndex ? (t = this.rectTick.left + this.rectTick.width - this.layout.digit.spaceX,
                            e = Object(T.d)(this.tickData, "close", c),
                            n = this.getColor(e, this.static.before),
                            x(this.context, t, i, Object(a.d)(e, this.static.coinunit), this.layout.digit.font, this.layout.digit.pixel, n, {
                                x: "end"
                            }),
                            i += r) : (t += l + this.closeLen + this.layout.digit.spaceX,
                            this.linkInfo.hideInfo || (e = Object(T.d)(this.tickData, "close", c),
                            n = this.getColor(e, this.static.before),
                            x(this.context, t, i, Object(a.d)(e, this.static.coinunit), this.layout.digit.font, this.layout.digit.pixel, n, {
                                x: "end"
                            })),
                            t += l + this.volLen + this.layout.digit.spaceX,
                            e = Object(T.d)(this.tickData, "decvol", c),
                            n = this.color.vol,
                            x(this.context, t, i, Object(a.f)(e, this.static.volzoom), this.layout.digit.font, this.layout.digit.pixel, n, {
                                x: "end"
                            }),
                            i += r)
                        }
                    }
                }
            }, {
                key: "drawGridLine",
                value: function () {
                    u(this.context, this.color.grid),
                    c(this.context, this.rectMain.left, this.rectMain.top, this.rectMain.width, this.rectMain.height);
                    var t, i, e, n, a = 1;
                    "normal" === this.style && (a = 5),
                    h(this.context, this.rectOrder.left, this.rectOrder.left + this.rectOrder.width, this.rectOrder.top + Math.floor(this.rectOrder.height / 2));
                    var s = ["①", "②", "③", "④", "⑤"]
                      , o = this.rectOrder.height / (2 * a);
                    t = g(this.context, "卖①", this.layout.title.font, this.layout.digit.height),
                    e = this.rectOrder.top + Math.floor((o - this.layout.digit.pixel) / 2);
                    for (var r = a - 1; 0 <= r; r--)
                        i = this.rectOrder.left + this.layout.digit.spaceX,
                        n = "卖" + s[r],
                        x(this.context, i, e, n, this.layout.digit.font, this.layout.digit.pixel, this.color.txt),
                        e += o;
                    for (var l = 0; l < a; l++)
                        i = this.rectOrder.left + this.layout.digit.spaceX,
                        n = "买" + s[l],
                        x(this.context, i, e, n, this.layout.digit.font, this.layout.digit.pixel, this.color.txt),
                        e += o;
                    if ("none" !== this.config.title.display) {
                        h(this.context, this.rectTitle.left, this.rectTitle.left + this.rectTitle.width, this.rectTitle.top),
                        h(this.context, this.rectTitle.left, this.rectTitle.left + this.rectTitle.width, this.rectTitle.top + this.rectTitle.height),
                        n = "normal" === this.style ? "分时成交 △" : "分时成交 ▽";
                        var f = g(this.context, n, this.layout.title.font, this.layout.digit.pixel);
                        i = this.rectTitle.left + (this.rectTitle.width - f) / 2,
                        e = this.rectTitle.top + 3 * this.scale,
                        x(this.context, i, e, n, this.layout.digit.font, this.layout.digit.pixel, this.color.txt)
                    }
                    return d(this.context),
                    t
                }
            }]) && jt(i.prototype, e),
            t
        }();
        function Xt(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var Yt = {
            showMode: "last",
            fixed: {
                min: -1,
                max: -1,
                left: 20,
                right: 20
            },
            locked: {
                index: -1,
                set: .5
            },
            minIndex: -1,
            maxIndex: -1,
            hotIndex: -1,
            showCursorLine: !1,
            moveIndex: -1,
            spaceX: 2,
            unitX: 5,
            rightMode: "no",
            hideInfo: !1
        }
          , Lt = function () {
              function t(i) {
                  !function (t, i) {
                      if (!(t instanceof i))
                          throw new TypeError("Cannot call a class as a function")
                  }(this, t),
                  this.context = i.mainCanvas.context,
                  this.cursorCanvas = i.cursorCanvas,
                  this.sysColor = i.sysColor,
                  this.father = void 0
              }
              var i, e;
              return i = t,
              (e = [{
                  key: "initChart",
                  value: function (t, i) {
                      this.linkInfo = Object(a.b)(Yt),
                      this.childCharts = {},
                      this.setDataLayer(t),
                      this.setEventLayer(i)
                  }
              }, {
                  key: "clear",
                  value: function () {
                      this.childCharts = {},
                      this.fastDraw = !1,
                      this.dataLayer.clearData(),
                      this.linkInfo = Object(a.b)(Yt)
                  }
              }, {
                  key: "getChart",
                  value: function (t) {
                      return this.childCharts[t]
                  }
              }, {
                  key: "getEventLayer",
                  value: function () {
                      return this.eventLayer
                  }
              }, {
                  key: "setEventLayer",
                  value: function (t) {
                      void 0 !== t && (this.eventLayer = t,
                      this.eventLayer.bindChart && this.eventLayer.bindChart(this))
                  }
              }, {
                  key: "getDataLayer",
                  value: function () {
                      return this.dataLayer
                  }
              }, {
                  key: "setDataLayer",
                  value: function (t) {
                      void 0 !== t && (((this.dataLayer = t).father = this).static = this.dataLayer.static)
                  }
              }, {
                  key: "bindData",
                  value: function (t, i) {
                      t.hotKey !== i && (this.linkInfo.showMode = "last",
                      this.linkInfo.minIndex = -1,
                      t.hotKey = i,
                      this.fastDrawEnd())
                  }
              }, {
                  key: "initData",
                  value: function (t, i) {
                      this.dataLayer.initData(t, i)
                  }
              }, {
                  key: "setData",
                  value: function (t, i, e) {
                      var n = e;
                      "string" == typeof e && (n = JSON.parse(e)),
                      this.dataLayer.setData(t, i, n),
                      this.fastDrawEnd()
                  }
              }, {
                  key: "getData",
                  value: function (t) {
                      if (this.fastDraw && void 0 !== this.fastBuffer[t])
                          return this.fastBuffer[t];
                      var i = this.dataLayer.getData(t, this.linkInfo.rightMode);
                      return this.fastDraw && (this.fastBuffer[t] = i),
                      i
                  }
              }, {
                  key: "readyData",
                  value: function (t, i) {
                      for (var e = 0; e < i.length; e++)
                          void 0 !== i[e].formula && (!this.fastDraw || this.fastDraw && void 0 === this.fastBuffer[i[e].formula.key]) && this.dataLayer.makeLineData({
                              data: t,
                              minIndex: this.linkInfo.minIndex,
                              maxIndex: this.linkInfo.maxIndex
                          }, i[e].formula.key, i[e].formula.command)
                  }
              }, {
                  key: "createChart",
                  value: function (t, i, e, n) {
                      var a;
                      switch (i) {
                          case "CHART.ORDER":
                              a = new Tt(this);
                              break;
                          case "CHART.LINE":
                          default:
                              a = new Ct(this)
                      }
                      return a.name = t,
                      (this.childCharts[t] = a).init(e, n),
                      a
                  }
              }, {
                  key: "onPaint",
                  value: function (t) {
                      for (var i in "function" == typeof this.context._beforePaint && this.context._beforePaint(),
                      this.fastDrawBegin(),
                      this.childCharts)
                          void 0 !== t ? this.childCharts[i] === t && this.childCharts[i].onPaint() : this.childCharts[i].onPaint();
                      "function" == typeof this.context._afterPaint && this.context._afterPaint()
                  }
              }, {
                  key: "fastDrawBegin",
                  value: function () {
                      this.fastDraw || (this.fastBuffer = [],
                      this.fastDraw = !0)
                  }
              }, {
                  key: "fastDrawEnd",
                  value: function () {
                      this.fastDraw = !1
                  }
              }, {
                  key: "setHideInfo",
                  value: function (t) {
                      void 0 !== t && t !== this.linkInfo.hideInfo && (this.linkInfo.hideInfo = t,
                      this.onPaint())
                  }
              }, {
                  key: "setColor",
                  value: function (t, i) {
                      for (var e in this.color = S(t),
                      void 0 === i && (i = this),
                      i.childCharts)
                          i.childCharts[e].color = this.color,
                          this.setColor(t, i.childCharts[e]);
                      for (var n in i.childDraws)
                          i.childDraws[n].color = this.color,
                          this.setColor(t, i.childDraws[n]);
                      for (var a in i.childLines)
                          i.childLines[a].color = this.color,
                          this.setColor(t, i.childLines[a]);
                      this.sysColor = t
                  }
              }, {
                  key: "setStandard",
                  value: function (t) {
                      var i;
                      i = t,
                      _.standard = i || "china",
                      this.setColor(this.sysColor),
                      this.onPaint()
                  }
              }]) && Xt(i.prototype, e),
              t
          }();
        function Et(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        function _t(t, i) {
            return Math.sqrt(t * t + i * i)
        }
        function St(t, i) {
            var e = {
                name: "touch"
            };
            if (t.offsetX && 0 === t.offsetX)
                e.offsetX = t.offsetX,
                e.offsetY = t.offsetY;
            else {
                var n = {
                    left: 0,
                    top: 0
                };
                i && "function" == typeof i.getBoundingClientRect && (n = i.getBoundingClientRect()),
                e.offsetX = t.pageX - n.left,
                e.offsetY = t.pageY - n.top
            }
            return e
        }
        function At(t) {
            return {
                offsetX: t.offsetX,
                offsetY: t.offsetY
            }
        }
        var Pt = function () {
            function t(i) {
                var e = i.father
                  , n = i.eventBuild
                  , a = i.isTouch;
                !function (i, e) {
                    if (!(i instanceof t))
                        throw new TypeError("Cannot call a class as a function")
                }(this),
                this.father = e,
                this.eventCanvas = e.eventCanvas,
                this.eventBuild = "function" == typeof n ? n : function (t) {
                    return t
                }
                ,
                this.isTouch = !!a,
                this.eventCanvas.addEventListener && this.eventCanvas.addEventListener("contextmenu", function (t) {
                    t.preventDefault()
                })
            }
            var i, e;
            return i = t,
            (e = [{
                key: "bindEvent",
                value: function () {
                    this.isTouch ? (this.addHandler("touchstart", this.touchstart.bind(this)),
                    this.addHandler("touchend", this.touchend.bind(this)),
                    this.addHandler("touchmove", this.touchmove.bind(this))) : (this.addHandler("mousemove", this.mousemove.bind(this)),
                    this.addHandler("mouseout", this.mouseout.bind(this)),
                    this.addHandler("mousewheel", this.mousewheel.bind(this)),
                    this.addHandler("mouseup", this.mouseup.bind(this)),
                    this.addHandler("mousedown", this.mousedown.bind(this)),
                    this.addHandler("keyup", this.keyup.bind(this)),
                    this.addHandler("keydown", this.keydown.bind(this)),
                    this.addHandler("click", this.click.bind(this)))
                }
            }, {
                key: "clearEvent",
                value: function () {
                    this.isTouch ? (this.clearHandler("touchstart", this.touchstart.bind(this)),
                    this.clearHandler("touchend", this.touchend.bind(this)),
                    this.clearHandler("touchmove", this.touchmove.bind(this))) : (this.clearHandler("mousemove", this.mousemove.bind(this)),
                    this.clearHandler("mouseout", this.mouseout.bind(this)),
                    this.clearHandler("mousewheel", this.mousewheel.bind(this)),
                    this.clearHandler("mouseup", this.mouseup.bind(this)),
                    this.clearHandler("mousedown", this.mousedown.bind(this)),
                    this.clearHandler("keyup", this.keyup.bind(this)),
                    this.clearHandler("keydown", this.keydown.bind(this)),
                    this.clearHandler("click", this.click.bind(this)))
                }
            }, {
                key: "addHandler",
                value: function (t, i) {
                    this.eventCanvas.addEventListener ? this.eventCanvas.addEventListener(t, i, !1) : this.eventCanvas.attachEvent ? this.eventCanvas.attachEvent("on" + t, i) : this.eventCanvas["on" + t] = i
                }
            }, {
                key: "clearHandler",
                value: function (t, i) {
                    this.eventCanvas.removeEventListener ? this.eventCanvas.removeEventListener(t, i, !1) : this.eventCanvas.deattachEvent ? this.eventCanvas.deattachEvent("on" + t, i) : this.eventCanvas["on" + t] = null
                }
            }, {
                key: "mousemove",
                value: function (t) {
                    this.father.emitEvent("onMouseMove", At(t))
                }
            }, {
                key: "mousein",
                value: function (t) {
                    this.father.emitEvent("onMouseIn", At(t))
                }
            }, {
                key: "mouseout",
                value: function (t) {
                    this.father.emitEvent("onMouseOut", At(t))
                }
            }, {
                key: "mousewheel",
                value: function (t) {
                    var i = At(t);
                    i.deltaY = t.deltaY,
                    this.father.emitEvent("onMouseWheel", i)
                }
            }, {
                key: "mouseup",
                value: function (t) {
                    this.father.emitEvent("onMouseUp", At(t))
                }
            }, {
                key: "mousedown",
                value: function (t) {
                    this.father.emitEvent("onMouseDown", At(t))
                }
            }, {
                key: "keyup",
                value: function (t) {
                    var i = At(t);
                    i.keyCode = t.keyCode,
                    this.father.emitEvent("onKeyUp", i)
                }
            }, {
                key: "keydown",
                value: function (t) {
                    var i = At(t);
                    i.keyCode = t.keyCode,
                    this.father.emitEvent("onKeyDown", i)
                }
            }, {
                key: "click",
                value: function (t) {
                    this.father.emitEvent("onClick", At(t))
                }
            }, {
                key: "touchstart",
                value: function (t) {
                    var i = this
                      , e = this.eventBuild(t);
                    this.__timestamp = new Date;
                    var n = e.touches ? e.touches[0] : e;
                    if (this.startX = n.pageX,
                    this.startY = n.pageY,
                    clearTimeout(this.longTapTimeout),
                    this.startTime = Date.now(),
                    1 < e.touches.length) {
                        var a = e.touches[1]
                          , s = Math.abs(a.pageX - this.startX)
                          , o = Math.abs(a.pageY - this.startY);
                        this.touchDistance = _t(s, o),
                        this.touchVector = {
                            x: a.pageX - this.startX,
                            y: a.pageY - this.startY
                        }
                    } else
                        this.longTapTimeout = setTimeout(function () {
                            i.father.emitEvent("onLongPress", St(n, e.target))
                        }, 600),
                        this.previousTouchPoint && Math.abs(this.startX - this.previousTouchPoint.startX) < 10 && Math.abs(this.startY - this.previousTouchPoint.startY) < 10 && Math.abs(this.startTime - this.previousTouchTime) < 300 && this.father.emitEvent("onDoubleClick", St(n, e.target)),
                        this.previousTouchTime = this.startTime,
                        this.previousTouchPoint = {
                            startX: this.startX,
                            startY: this.startY
                        }
                }
            }, {
                key: "touchend",
                value: function (t) {
                    var i = this.eventBuild(t);
                    clearTimeout(this.longTapTimeout);
                    var e = i.changedTouches ? i.changedTouches[0] : i
                      , n = Date.now();
                    null !== this.moveX && 10 < Math.abs(this.moveX - this.startX) || null !== this.moveY && 10 < Math.abs(this.moveY - this.startY) ? n - this.startTime < 500 && this.father.emitEvent("onSwipe", St(e, i.target)) : n - this.startTime < 2e3 && (n - this.startTime < 500 && this.father.emitEvent("onClick", St(e, i.target)),
                    500 < n - this.startTime && this.father.emitEvent("onLongPress", St(e, i.target))),
                    this.startX = this.startY = this.moveX = this.moveY = null,
                    this.previousPinchScale = 1,
                    this.father.emitEvent("onMouseOut", St(e, i.target))
                }
            }, {
                key: "touchmove",
                value: function (t) {
                    var i = this.eventBuild(t);
                    if (new Date - this.__timestamp < 150)
                        return i;
                    var e = Date.now();
                    if (1 < i.touches.length) {
                        var n = _t(Math.abs(i.touches[0].pageX - i.touches[1].pageX), Math.abs(i.touches[0].pageY - i.touches[1].pageY));
                        if (this.touchDistance) {
                            var a = n / this.touchDistance
                              , s = St(i.touches ? i.touches[0] : i, i.target);
                            90 < e - this.startTime && this.previousPinchScale && (s.scale = a - this.previousPinchScale,
                            .01 < Math.abs(s.scale) && this.father.emitEvent("onPinch", s),
                            this.startTime = Date.now()),
                            this.previousPinchScale = a
                        }
                        if (this.touchVector) {
                            var o = {
                                x: i.touches[1].pageX - i.touches[0].pageX,
                                y: i.touches[1].pageY - i.touches[0].pageY
                            }
                              , h = function (t, i) {
                                  var e, n, a = (e = t).x * (n = i).y - n.x * e.y;
                                  a = 0 < a ? -1 : 1;
                                  var s = _t(t.x, t.y) * _t(i.x, i.y);
                                  if (0 === s)
                                      return 0;
                                  var o = (t.x * i.x + t.y * i.y) / s;
                                  return 1 < o && (o = 1),
                                  o < -1 && (o = -1),
                                  Math.acos(o) * a * 180 / Math.PI
                              }(o, this.touchVector);
                            this.father.emitEvent("onRotate", {
                                angle: h
                            }),
                            this.touchVector.x = o.x,
                            this.touchVector.y = o.y
                        }
                    } else {
                        clearTimeout(this.longTapTimeout);
                        var r = i.touches ? i.touches[0] : i
                          , l = null === this.moveX ? 0 : r.pageX - this.moveX
                          , c = null === this.moveY ? 0 : r.pageY - this.moveY
                          , f = St(r, i.target);
                        f.deltaX = l,
                        f.deltaY = c,
                        this.father.emitEvent("onMouseMove", f),
                        this.moveX = r.pageX,
                        this.moveY = r.pageY
                    }
                    "function" == typeof i.preventDefault && i.preventDefault()
                }
            }]) && Et(i.prototype, e),
            t
        }();
        function Ft(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        function Rt(t) {
            var i = {};
            if (t && Array.isArray(t.touches)) {
                i.touches = [];
                for (var e = 0; e < t.touches.length; e++) {
                    var n = t.touches[e];
                    i.touches.push({
                        pageX: n.x,
                        pageY: n.y,
                        offsetX: n.x,
                        offsetY: n.y
                    })
                }
            }
            if (t && Array.isArray(t.changedTouches)) {
                i.changedTouches = [];
                for (var a = 0; a < t.changedTouches.length; a++) {
                    var s = t.changedTouches[a];
                    i.changedTouches.push({
                        pageX: s.x,
                        pageY: s.y,
                        offsetX: s.x,
                        offsetY: s.y
                    })
                }
            }
            return i
        }
        var Nt = function () {
            function t(i) {
                !function (i, e) {
                    if (!(i instanceof t))
                        throw new TypeError("Cannot call a class as a function")
                }(this),
                this.eventCanvas = i.cursorCanvas.canvas,
                this.eventPlatform = i.eventPlatform || "web",
                this.scale = i.scale;
                var e = {
                    father: this
                };
                "react-native" === this.eventPlatform ? e.isTouch = !0 : "web" === this.eventPlatform ? e.isTouch = "ontouchend" in document : "mina" === this.eventPlatform && (e.isTouch = !0,
                e.eventBuild = Rt),
                this.eventSource = new Pt(e),
                this.eventSource.bindEvent()
            }
            var i, e;
            return i = t,
            (e = [{
                key: "bindChart",
                value: function (t) {
                    this.firstChart = t,
                    this.HotWin = void 0
                }
            }, {
                key: "clearEvent",
                value: function () {
                    this.eventSource.clearEvent()
                }
            }, {
                key: "emitEvent",
                value: function (t, i) {
                    if ("onMouseOut" === t || "onMouseMove" === t)
                        return this.boardEvent(this.firstChart, t, i),
                        void (this.HotWin = void 0);
                    var e = this.getMousePos(i)
                      , n = [];
                    for (var s in this.firstChart.childCharts)
                        if (Object(a.r)(this.firstChart.childCharts[s].rectMain, e)) {
                            this.findEventPath(n, this.firstChart.childCharts[s], e);
                            break
                        }
                    if (!(n.length < 1))
                        for (var o = Object(a.b)(i), h = n.length - 1; 0 <= h && (void 0 === n[h][t] || (o.mousePos = {
                            x: e.x,
                            y: e.y
                        },
                        n[h][t](o),
                        !o.break)) ; h--)
                            ;
                }
            }, {
                key: "boardEvent",
                value: function (t, i, e, n) {
                    var s = Object(a.b)(e)
                      , o = this.getMousePos(e);
                    for (var h in t.childCharts)
                        if (t.childCharts[h] !== n && void 0 !== t.childCharts[h][i] && (s.mousePos = {
                            x: o.x,
                            y: o.y
                        },
                        t.childCharts[h][i](s),
                        s.break))
                            break
                }
            }, {
                key: "findEventPath",
                value: function (t, i, e) {
                    if (t.push(i),
                    void 0 !== i.childCharts)
                        for (var n in i.childCharts)
                            Object(a.r)(i.childCharts[n].rectMain, e) && this.findEventPath(t, i.childCharts[n], e)
                }
            }, {
                key: "getMousePos",
                value: function (t) {
                    return {
                        x: t.offsetX * this.scale,
                        y: t.offsetY * this.scale
                    }
                }
            }]) && Ft(i.prototype, e),
            t
        }()
          , zt = e(3);
        function Ht(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var Kt = function () {
            function t() {
                !function (t, i) {
                    if (!(t instanceof i))
                        throw new TypeError("Cannot call a class as a function")
                }(this, t),
                this.static = {
                    stktype: 1,
                    volunit: 100,
                    coinunit: 100,
                    decimal: 2,
                    before: 1e3,
                    stophigh: 1100,
                    stoplow: 900
                }
            }
            var i, e;
            return i = t,
            (e = [{
                key: "initData",
                value: function (t, i) {
                    this.formula = new zt.a,
                    this.clearData(),
                    this.tradeTime = void 0 === i ? [{
                        begin: 930,
                        end: 1130
                    }, {
                        begin: 1300,
                        end: 1500
                    }] : i,
                    this.tradeDate = void 0 === t ? Object(a.h)() : t
                }
            }, {
                key: "clearData",
                value: function () {
                    this.InData = [],
                    this.OutData = []
                }
            }, {
                key: "setData",
                value: function (t, i, e) {
                    switch (void 0 === e && (e = []),
                    void 0 === this.InData[t] && (this.InData[t] = {}),
                    t) {
                        case "DAY5":
                            e = Object(T.a)(e, this.static.coinzoom, this.tradeDate, this.tradeTime);
                            break;
                        case "NOW":
                        case "ENOW":
                            this.flushNowData(t, e);
                            break;
                        case "MIN":
                        case "DAY":
                            e = Object(T.b)(e, this.tradeDate);
                            break;
                        case "INFO":
                            this.static = Object(T.p)(lt.FIELD_INFO, e)
                    }
                    this.InData[t] = {
                        key: t,
                        fields: i
                    },
                    this.InData[t].value = Object(a.a)(e)
                }
            }, {
                key: "flushTick",
                value: function (t, i) {
                    Object(T.k)(this.InData.TICK) < 1 ? 0 < t[i.vol] && (this.InData.TICK = {
                        key: "TICK",
                        fields: lt.FIELD_TICK,
                        value: [t[i.time], t[i.close], t[i.vol]]
                    }) : (this.InData.TICK.value[this.InData.TICK.value.length - 1][lt.FIELD_TICK.vol] < t[i.vol] || this.InData.TICK.value[this.InData.TICK.value.length - 1][lt.FIELD_TICK.close] !== t[i.close]) && this.InData.TICK.value.push([t[i.time], t[i.close], t[i.vol]])
                }
            }, {
                key: "flushMin",
                value: function (t, i) {
                    if (void 0 === this.InData.MIN)
                        this.InData.MIN = {
                            key: "MIN",
                            fields: lt.FIELD_MIN,
                            value: [Object(T.h)(t[i.time], this.tradeTime), t[i.open], t[i.high], t[i.low], t[i.close], t[i.vol], t[i.money]]
                        };
                    else {
                        var e = Object(T.h)(t[i.time], this.tradeTime)
                          , n = Object(T.f)(this.InData.MIN, e);
                        "old" === n.status ? (this.InData.MIN.value[n.index][i.high] < t[i.close] && (this.InData.MIN.value[n.index][i.high] = t[i.close]),
                        this.InData.MIN.value[n.index][i.low] > t[i.close] && (this.InData.MIN.value[n.index][i.low] = t[i.close]),
                        this.InData.MIN.value[n.index][i.close] = t[i.close],
                        this.InData.MIN.value[n.index][i.vol] = t[i.vol],
                        this.InData.MIN.value[n.index][i.money] = t[i.money]) : "new" === n.status && this.InData.MIN.value.push([e, t[i.close], t[i.close], t[i.close], t[i.close], t[i.vol], t[i.money]])
                    }
                }
            }, {
                key: "flushNowData",
                value: function (t, i) {
                    if (!(i.length < 1)) {
                        var e = lt.FIELD_NOW;
                        "ENOW" === t && (e = lt.FIELD_ENOW),
                        Object(T.c)(i, e) || (this.flushTick(i, e),
                        this.flushMin(i, e))
                    }
                }
            }, {
                key: "getData",
                value: function (t, i) {
                    switch (t) {
                        case "DAY":
                            this.OutData.DAY = {
                                key: t,
                                fields: lt.FIELD_DAY
                            },
                            this.OutData.DAY.value = this.mergeDay(this.InData.DAY, this.InData.NOW, i);
                            break;
                        case "WEEK":
                            this.OutData.WEEK = {
                                key: t,
                                fields: lt.FIELD_DAY
                            },
                            this.OutData.WEEK.value = this.mergeWeek(this.InData.DAY, this.InData.NOW, i);
                            break;
                        case "MON":
                            this.OutData.MON = {
                                key: t,
                                fields: lt.FIELD_DAY
                            },
                            this.OutData.MON.value = this.mergeMon(this.InData.DAY, this.InData.NOW, i);
                            break;
                        case "DAY5":
                            this.OutData.DAY5 = {
                                key: t,
                                fields: lt.FIELD_DAY5
                            },
                            this.OutData.DAY5.value = this.mergeDay5(this.InData.DAY5, this.InData.MIN);
                            break;
                        case "M5":
                        case "M15":
                        case "M30":
                        case "M60":
                            this.OutData[t] = {
                                key: t,
                                fields: lt.FIELD_DAY
                            },
                            this.OutData[t].value = this.makeMinute(t, this.InData[t], this.InData.MIN, i);
                            break;
                        case "MIN":
                            this.OutData[t] = {
                                key: t,
                                fields: lt.FIELD_MIN
                            },
                            this.OutData[t].value = this.updateMinute(this.InData[t])
                    }
                    return this.OutData[t] ? this.OutData[t] : this.InData[t]
                }
            }, {
                key: "updateMinute",
                value: function (t) {
                    for (var i, e = Object(a.a)(t.value), n = 0; n < e.length; n++)
                        0 === this.static.stktype && (0 === n ? i = e[n][lt.FIELD_MIN.vol] * e[n][lt.FIELD_MIN.close] : i += (e[n][lt.FIELD_MIN.vol] - e[n - 1][lt.FIELD_MIN.vol]) * e[n][lt.FIELD_MIN.close],
                        e[n][lt.FIELD_MIN.allmoney] = i);
                    return e
                }
            }, {
                key: "mergeDay",
                value: function (t, i, e) {
                    var n = Object(a.a)(t.value);
                    if (void 0 !== i && !Object(T.c)(i.value, i.fields)) {
                        var s = Object(T.e)(t, Object(a.h)(i.value[i.fields.time]));
                        s.finded ? n[s.index] = [Object(a.h)(i.value[i.fields.time]), i.value[i.fields.open], i.value[i.fields.high], i.value[i.fields.low], i.value[i.fields.close], i.value[i.fields.vol], i.value[i.fields.money]] : n.push([Object(a.h)(i.value[i.fields.time]), i.value[i.fields.open], i.value[i.fields.high], i.value[i.fields.low], i.value[i.fields.close], i.value[i.fields.vol], i.value[i.fields.money]])
                    }
                    return this.InData.RIGHT && "none" !== e && (n = Object(T.n)(n, this.InData.RIGHT.value, e, 0, n.length - 1)),
                    n
                }
            }, {
                key: "mergeWeek",
                value: function (t, i, e) {
                    var n = this.mergeDay(t, i, e);
                    return Object(T.m)(n)
                }
            }, {
                key: "mergeMon",
                value: function (t, i, e) {
                    var n = this.mergeDay(t, i, e);
                    return Object(T.l)(n)
                }
            }, {
                key: "mergeDay5",
                value: function (t, i) {
                    var e = [];
                    if (void 0 !== t && !Object(a.t)(t.value) && (e = Object(a.a)(t.value),
                    Object(a.h)(t.value[t.value.length - 1][t.fields.time]) === this.tradeDate))
                        return e;
                    if (void 0 === i || Object(a.t)(i.value))
                        return e;
                    for (var n, s = 4 * Object(T.j)(this.tradeTime), o = 0; o < i.value.length; o++)
                        0 === this.static.stktype ? 0 === o ? n = i.value[o][i.fields.vol] * i.value[o][i.fields.close] : n += (i.value[o][i.fields.vol] - i.value[o - 1][i.fields.vol]) * i.value[o][i.fields.close] : n = i.value[o][i.fields.money],
                        e.push([Object(T.g)(i.value[o][i.fields.idx], this.tradeTime, this.tradeDate), i.value[o][i.fields.close], 0 === o ? i.value[o][i.fields.vol] : i.value[o][i.fields.vol] - i.value[o - 1][i.fields.vol], s + i.value[o][i.fields.idx], i.value[o][i.fields.vol], n]);
                    return e
                }
            }, {
                key: "makeMinute",
                value: function (t, i, e, n) {
                    var s = [];
                    if (void 0 !== i && !Object(a.t)(i.value) && (s = Object(a.a)(i.value),
                    s = Object(T.o)(s, this.static.coinzoom, this.InData.RIGHT.value, n, 0, s.length - 1),
                    Object(a.h)(i.value[i.value.length - 1][i.fields.time]) === this.tradeDate))
                        return s;
                    if (void 0 !== e && !Object(a.t)(e.value)) {
                        var o = 5;
                        "M15" === t && (o = 15),
                        "M30" === t && (o = 30),
                        "M60" === t && (o = 60),
                        s = this.mergeNowMinToMin(s, e, o)
                    }
                    return s
                }
            }, {
                key: "mergeNowMinToMin",
                value: function (t, i, e) {
                    for (var n = [], s = 0, o = 0, h = !1, r = 4, l = 0; l < i.value.length; l++) {
                        var c = i.value[l][i.fields.idx];
                        c < 0 || (r < c ? (h && (n[i.fields.vol] = i.value[l][i.fields.vol] - s,
                        n[i.fields.money] = i.value[l][i.fields.money] - o,
                        s = i.value[l][i.fields.vol],
                        o = i.value[l][i.fields.money],
                        n[i.fields.time] = Object(T.g)(r, this.tradeTime, this.tradeDate),
                        t.push(Object(a.a)(n))),
                        r = (Math.floor(c / e) + 1) * e - 1,
                        n[i.fields.open] = i.value[l][i.fields.open],
                        n[i.fields.high] = i.value[l][i.fields.high],
                        n[i.fields.low] = i.value[l][i.fields.low],
                        n[i.fields.close] = i.value[l][i.fields.close],
                        h = !0) : h ? (n[i.fields.high] = n[i.fields.high] > i.value[l][i.fields.high] ? n[i.fields.high] : i.value[l][i.fields.high],
                        n[i.fields.low] = n[i.fields.low] < i.value[l][i.fields.low] || 0 === i.value[l][i.fields.low] ? n[i.fields.low] : i.value[l][i.fields.low],
                        n[i.fields.close] = i.value[l][i.fields.close]) : (n[i.fields.open] = i.value[l][i.fields.open],
                        n[i.fields.high] = i.value[l][i.fields.high],
                        n[i.fields.low] = i.value[l][i.fields.low],
                        n[i.fields.close] = i.value[l][i.fields.close],
                        h = !0))
                    }
                    return h && (n[i.fields.vol] = i.value[i.value.length - 1][i.fields.vol] - s,
                    n[i.fields.money] = i.value[i.value.length - 1][i.fields.money] - o,
                    n[i.fields.time] = Object(T.g)(r, this.tradeTime, this.tradeDate),
                    t.push(Object(a.a)(n))),
                    t
                }
            }, {
                key: "makeLineData",
                value: function (t, i, e) {
                    var n = this.formula.runSingleStock(t, e);
                    return void 0 === this.OutData[i] ? this.OutData[i] = {
                        outkey: i,
                        fields: lt.FIELD_ILINE,
                        value: n
                    } : this.OutData[i].value = n,
                    this.OutData[i]
                }
            }]) && Ht(i.prototype, e),
            t
        }();
        function Bt(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var Wt = {
            start: 1,
            period: 2,
            buy: 3,
            target: 4,
            stoploss: 5,
            status: 6,
            uid: 11,
            focused: 12
        }
          , Gt = {
              CHART_SEER: {
                  zoomInfo: {
                      index: 3,
                      list: [1, 2, 4, 5, 7, 9, 12, 15, 19]
                  },
                  scroll: {
                      display: "none"
                  },
                  title: {
                      display: "none"
                  },
                  axisX: {
                      lines: 0,
                      display: "both",
                      type: "normal",
                      format: "date"
                  },
                  axisY: {
                      lines: 3,
                      left: {
                          display: "both",
                          middle: "none",
                          format: "price"
                      },
                      right: {
                          display: "both",
                          middle: "none",
                          format: "price"
                      }
                  },
                  lines: [{
                      className: K,
                      extremum: {
                          method: "normal",
                          maxvalue: ["high"],
                          minvalue: ["low"]
                      }
                  }, {
                      className: V
                  }, {
                      className: function () {
                          function t(i, e) {
                              !function (i, e) {
                                  if (!(i instanceof t))
                                      throw new TypeError("Cannot call a class as a function")
                              }(this),
                              F(this, i),
                              this.rectMain = e,
                              this.linkInfo = i.father.linkInfo,
                              this.source = i.father,
                              this.static = i.father.static,
                              this.maxmin = i.maxmin,
                              this.layout = i.layout
                          }
                          var i, e;
                          return i = t,
                          (e = [{
                              key: "getSeerPos",
                              value: function (t, i) {
                                  var e = t - this.linkInfo.minIndex;
                                  if (e < 0 || t > this.linkInfo.maxIndex)
                                      return {
                                          visible: !1
                                      };
                                  var n = this.rectMain.left + e * (this.linkInfo.unitX + this.linkInfo.spaceX) + Math.floor(this.linkInfo.unitX / 2)
                                    , a = i;
                                  return void 0 === i && (a = Object(T.d)(this.data, "close", t)),
                                  {
                                      visible: !0,
                                      xx: n,
                                      yy: this.rectMain.top + Math.round((this.maxmin.max - a) * this.maxmin.unitY)
                                  }
                              }
                          }, {
                              key: "drawHotSeer",
                              value: function (t) {
                                  var i = X(this.data, Object(T.d)(this.sourceSeer, "start", t), "time", "forword");
                                  -1 === i && (i = this.linkInfo.maxIndex);
                                  var e = i - this.linkInfo.minIndex;
                                  if (!(e < 0)) {
                                      var n = this.rectMain.left + e * (this.linkInfo.unitX + this.linkInfo.spaceX) + Math.floor(this.linkInfo.unitX / 2)
                                        , s = Object(T.d)(this.sourceSeer, "status", t)
                                        , o = Object(T.d)(this.sourceSeer, "buy", t)
                                        , h = o
                                        , r = this.rectMain.top + Math.round((this.maxmin.max - h) * this.maxmin.unitY)
                                        , l = this.linkInfo.hideInfo ? "买点" : "买点:" + Object(a.d)(h, this.static.coinunit);
                                      0 === o && (l = "停牌中",
                                      h = Object(T.d)(this.data, "close", i),
                                      r = this.rectMain.top + Math.round((this.maxmin.max - h) * this.maxmin.unitY));
                                      var c = this.color.vol;
                                      Object(a.o)(s, [1, 20, 300]) ? c = this.color.txt : Object(a.o)(s, [2]) && (l = "停牌中"),
                                      O(this.context, {
                                          linew: this.scale,
                                          xx: n,
                                          yy: r,
                                          right: this.rectMain.left + this.rectMain.width + this.layout.offset.right - 2 * this.scale,
                                          clr: c,
                                          bakclr: this.color.back,
                                          font: this.layout.title.font,
                                          pixel: this.layout.title.pixel,
                                          spaceX: 4 * this.scale,
                                          spaceY: 3 * this.scale,
                                          x: "start",
                                          y: "middle"
                                      }, [{
                                          txt: l,
                                          set: 100,
                                          display: !this.linkInfo.hideInfo
                                      }]);
                                      var f = Object(T.d)(this.sourceSeer, "period", t)
                                        , m = f - (this.linkInfo.maxIndex - i)
                                        , x = " 周期:[" + f + "天]";
                                      0 < m && (x += " 剩余:[" + m + "天]"),
                                      function (t, i, e) {
                                          u(t, i.clr),
                                          v(t, i.xx, i.yy, i.xx, i.bottom - i.pixel / 2, 7),
                                          d(t);
                                          var n = i.spaceX || 2 * i.linew
                                            , a = i.spaceY || i.linew;
                                          i.height = i.bottom - i.yy;
                                          for (var s = 0; s < e.length; s++)
                                              if ("false" !== e[s].display) {
                                                  var o = i.yy + i.height * e[s].set / 100;
                                                  if ("arc" === e[s].txt)
                                                      o + e[s].maxR > i.bottom && (o = i.bottom - e[s].maxR),
                                                      M(t, i.xx, o, {
                                                          r: e[s].maxR,
                                                          clr: i.clr
                                                      }, {
                                                          r: e[s].minR,
                                                          clr: i.bakclr
                                                      });
                                                  else {
                                                      var h = p(t, e[s].txt, {
                                                          font: i.font,
                                                          pixel: i.pixel,
                                                          spaceX: n,
                                                          spaceY: a
                                                      });
                                                      o + h.height > i.bottom && (o = i.bottom - h.height);
                                                      var r = i.xx;
                                                      i.left && r < i.left + h.width / 2 && (r = i.left + h.width / 2),
                                                      b(t, r, o, e[s].txt, {
                                                          font: i.font,
                                                          pixel: i.pixel,
                                                          clr: i.clr,
                                                          bakclr: i.bakclr,
                                                          x: "center",
                                                          y: "middle",
                                                          spaceX: n,
                                                          spaceY: a
                                                      })
                                                  }
                                              }
                                      }(this.context, {
                                          linew: this.scale,
                                          xx: n,
                                          yy: r,
                                          left: this.rectMain.left,
                                          bottom: this.rectMain.top + this.rectMain.height + this.layout.offset.bottom + 2 * this.scale,
                                          clr: c,
                                          bakclr: this.color.back,
                                          font: this.layout.title.font,
                                          pixel: this.layout.title.pixel,
                                          spaceX: 2 * this.scale,
                                          paceY: 2 * this.scale
                                      }, [{
                                          txt: "arc",
                                          set: 0,
                                          minR: 0,
                                          maxR: 3 * this.scale,
                                          display: !0
                                      }, {
                                          txt: Object(T.d)(this.sourceSeer, "start", t) + x,
                                          set: 100,
                                          display: !0
                                      }]),
                                      this.drawTransRect(this.rectMain.left, n);
                                      var y = n + f * (this.linkInfo.spaceX + this.linkInfo.unitX);
                                      if (this.drawTransRect(y, this.rectMain.left + this.rectMain.width),
                                      0 !== o) {
                                          var g;
                                          h = Object(T.d)(this.sourceSeer, "stoploss", t);
                                          var k = this.rectMain.top + Math.round((this.maxmin.max - h) * this.maxmin.unitY);
                                          g = k - r > 1.5 * this.layout.title.pixel ? [{
                                              txt: "arc",
                                              set: 0,
                                              minR: 0,
                                              maxR: 2 * this.scale,
                                              display: !0
                                          }, {
                                              txt: this.linkInfo.hideInfo ? "止损" : "止损:" + Object(a.d)(h, this.static.coinunit),
                                              set: 100,
                                              display: !this.linkInfo.hideInfo
                                          }] : [{
                                              txt: "arc",
                                              set: 0,
                                              minR: 0,
                                              maxR: 2 * this.scale,
                                              display: !0
                                          }, {
                                              txt: "arc",
                                              set: 100,
                                              minR: 0,
                                              maxR: 1 * this.scale,
                                              display: !0
                                          }],
                                          Object(a.q)(this.rectMain, r) && O(this.context, {
                                              linew: this.scale,
                                              xx: n,
                                              yy: k,
                                              right: this.rectMain.left + this.rectMain.width + this.layout.offset.right - 2 * this.scale,
                                              clr: this.color.green,
                                              bakclr: this.color.back,
                                              font: this.layout.title.font,
                                              pixel: this.layout.title.pixel,
                                              spaceX: 4 * this.scale,
                                              spaceY: 3 * this.scale,
                                              x: "start",
                                              y: "middle"
                                          }, g),
                                          h = Object(T.d)(this.sourceSeer, "target", t),
                                          g = r - (k = this.rectMain.top + Math.round((this.maxmin.max - h) * this.maxmin.unitY)) > 1.5 * this.layout.title.pixel ? [{
                                              txt: "arc",
                                              set: 0,
                                              minR: 0,
                                              maxR: 2 * this.scale,
                                              display: !0
                                          }, {
                                              txt: this.linkInfo.hideInfo ? "目标" : "目标:" + Object(a.d)(h, this.static.coinunit),
                                              set: 100,
                                              display: !this.linkInfo.hideInfo
                                          }] : [{
                                              txt: "arc",
                                              set: 0,
                                              minR: 0,
                                              maxR: 2 * this.scale,
                                              display: !0
                                          }, {
                                              txt: "arc",
                                              set: 100,
                                              minR: 0,
                                              maxR: 2 * this.scale,
                                              display: !0
                                          }],
                                          Object(a.q)(this.rectMain, r) && O(this.context, {
                                              linew: this.scale,
                                              xx: n,
                                              yy: k,
                                              right: this.rectMain.left + this.rectMain.width + this.layout.offset.right - 2 * this.scale,
                                              clr: this.color.red,
                                              bakclr: this.color.back,
                                              font: this.layout.title.font,
                                              pixel: this.layout.title.pixel,
                                              spaceX: 4 * this.scale,
                                              spaceY: 3 * this.scale,
                                              x: "start",
                                              y: "middle"
                                          }, g);
                                          var w = Object(T.d)(this.sourceSeer, "stop", t);
                                          if (Object(a.o)(s, [101, 102, 200, 201, 202, 300])) {
                                              var I = X(this.data, w, "time", "forword") - this.linkInfo.minIndex
                                                , D = this.rectMain.left + I * (this.linkInfo.unitX + this.linkInfo.spaceX) + Math.floor(this.linkInfo.unitX / 2);
                                              D > this.rectMain.left && D < this.rectMain.left + this.rectMain.width - 4 * this.scale && (c = this.color.vol,
                                              h = Object(T.d)(this.sourceSeer, "buy", t),
                                              k = this.rectMain.top + Math.round((this.maxmin.max - h) * this.maxmin.unitY),
                                              Object(a.o)(s, [102, 202]) ? (c = this.color.green,
                                              h = Object(T.d)(this.sourceSeer, "stoploss", t),
                                              k = this.rectMain.top + Math.round((this.maxmin.max - h) * this.maxmin.unitY)) : Object(a.o)(s, [101, 201]) ? (c = this.color.red,
                                              h = Object(T.d)(this.sourceSeer, "target", t),
                                              k = this.rectMain.top + Math.round((this.maxmin.max - h) * this.maxmin.unitY)) : Object(a.o)(s, [300]) && (c = this.color.txt),
                                              M(this.context, D, k, {
                                                  r: 3 * this.scale,
                                                  clr: c
                                              }))
                                          }
                                      }
                                  }
                              }
                          }, {
                              key: "filterSeer",
                              value: function () {
                                  for (var t = {}, i = 0; i < this.sourceSeer.value.length; i++) {
                                      var e = Object(T.d)(this.sourceSeer, "start", i)
                                        , n = X(this.data, e, "time", "forword");
                                      -1 === n && (n = this.linkInfo.maxIndex),
                                      void 0 === t[n] && (t[n] = {
                                          nos: [],
                                          uids: []
                                      }),
                                      t[n].name = "SEER" + n,
                                      t[n].date = Object(T.d)(this.data, "time", n),
                                      t[n].nos.push(i),
                                      t[n].uids.push(Object(T.d)(this.sourceSeer, "uid", i)),
                                      Object(a.o)(Object(T.d)(this.sourceSeer, "uid", i), this.hotSeer.value) && (t[n].focused = !0,
                                      t[n].hotIdx = i)
                                  }
                                  return t
                              }
                          }, {
                              key: "beforeLocation",
                              value: function () {
                                  this.linkInfo.rightMode = "forword",
                                  this.data = this.source.getData(this.father.hotKey);
                                  var t = this.data.value[this.data.value.length - 1][this.data.fields.time];
                                  if (this.sourceSeer = this.source.getData("SEER"),
                                  this.hotSeer = this.source.getData("SEERHOT"),
                                  this.sourceSeer.value.length < 1)
                                      return 0;
                                  if (void 0 === this.hotSeer && (this.hotSeer = {
                                      value: [Object(T.d)(this.sourceSeer, "uid", 0)]
                                  }),
                                  this.sourceSeer.value.length < 1)
                                      return 0;
                                  var i = {
                                      max: Object(T.d)(this.sourceSeer, "start", 0),
                                      min: Object(T.d)(this.sourceSeer, "start", this.sourceSeer.value.length - 1)
                                  }
                                    , e = this.source.getData("RIGHT");
                                  if (void 0 !== e) {
                                      for (var n = Object(a.a)(this.sourceSeer.value), s = 0; s < n.length; s++)
                                          n[s][Wt.buy] = Object(T.i)(i.min, t, n[s][Wt.buy], e.value),
                                          n[s][Wt.target] = Object(T.i)(i.min, t, n[s][Wt.target], e.value),
                                          n[s][Wt.stoploss] = Object(T.i)(i.min, t, n[s][Wt.stoploss], e.value);
                                      this.sourceSeer = {
                                          key: "SEER",
                                          fields: Wt,
                                          value: n
                                      }
                                  }
                                  for (var o in this.showSeer = this.filterSeer(),
                                  this.showSeer)
                                      this.showSeer[o].chart = this.father.createButton(this.showSeer[o].name),
                                      i.max = i.max < this.showSeer[o].date ? this.showSeer[o].date : i.max,
                                      i.min = i.min > this.showSeer[o].date ? this.showSeer[o].date : i.min;
                                  this.hotKey = "SEERDAY",
                                  this.data = {
                                      key: "SEERDAY",
                                      fields: lt.FIELD_DAY,
                                      value: this.data.value
                                  },
                                  this.linkInfo.showMode = "fixed",
                                  this.linkInfo.fixed.min = i.min,
                                  this.linkInfo.fixed.max = i.max
                              }
                          }, {
                              key: "drawTransRect",
                              value: function (t, i) {
                                  if (!(i < t)) {
                                      var e = j(this.color.grid, .5);
                                      f(this.context, t, this.rectMain.top, i - t, this.rectMain.height, e)
                                  }
                              }
                          }, {
                              key: "onPaint",
                              value: function (t) {
                                  var i = this
                                    , e = function (t) {
                                        var e = void 0;
                                        if (i.showSeer[t].uids.length < 1)
                                            return "continue";
                                        1 === i.showSeer[t].uids.length && (e = Object(T.d)(i.sourceSeer, "buy", i.showSeer[t].nos[0])),
                                        !0 === i.showSeer[t].focused && (i.father.setHotButton(i.showSeer[t].chart),
                                        1 === i.hotSeer.value.length && (e = Object(T.d)(i.sourceSeer, "buy", i.showSeer[t].hotIdx)));
                                        var n = i.getSeerPos(t, e)
                                          , s = "*";
                                        i.showSeer[t].uids.length < 10 && (s = i.showSeer[t].uids.length.toString());
                                        var o = i.layout.symbol.size / 2;
                                        i.showSeer[t].chart.init({
                                            rectMain: {
                                                left: n.xx - o,
                                                top: i.showSeer[t].focused ? n.yy - o - 2 * o : n.yy - o,
                                                width: 2 * o,
                                                height: i.showSeer[t].focused ? 2 * o + 2 * o : 2 * o
                                            },
                                            config: {
                                                translucent: !0,
                                                visible: n.visible,
                                                status: i.showSeer[t].focused ? "focused" : "enabled",
                                                shape: "set"
                                            },
                                            info: [{
                                                map: s
                                            }]
                                        }, function (e) {
                                            0 < Object(a.s)(i.showSeer[t].uids, i.hotSeer.value).length ? (i.hotSeer.value = [],
                                            i.father.callback({
                                                event: "seerclick",
                                                data: []
                                            })) : (i.hotSeer.value = i.showSeer[t].uids,
                                            i.father.callback({
                                                event: "seerclick",
                                                data: i.showSeer[t].uids
                                            })),
                                            i.source.onPaint()
                                        })
                                    };
                                  for (var n in this.showSeer)
                                      e(n);
                                  if (1 === this.hotSeer.value.length)
                                      for (n = 0; n < this.sourceSeer.value.length; n++)
                                          Object(T.d)(this.sourceSeer, "uid", n) === this.hotSeer.value[0] && this.drawHotSeer(n)
                              }
                          }]) && Bt(i.prototype, e),
                          t
                      }()
                  }]
              },
              FIELD_SEER: Wt
          };
        function Vt(t, i) {
            for (var e = 0; e < i.length; e++) {
                var n = i[e];
                n.enumerable = n.enumerable || !1,
                n.configurable = !0,
                "value" in n && (n.writable = !0),
                Object.defineProperty(t, n.key, n)
            }
        }
        var Ut = function () {
            function t() {
                !function (t, i) {
                    if (!(t instanceof i))
                        throw new TypeError("Cannot call a class as a function")
                }(this, t),
                this.handlers = {}
            }
            var i, e;
            return i = t,
            (e = [{
                key: "emit",
                value: function (t) {
                    for (var i = this, e = arguments.length, n = new Array(1 < e ? e - 1 : 0), a = 1; a < e; a++)
                        n[a - 1] = arguments[a];
                    this.handlers[t] && this.handlers[t].forEach(function (t) {
                        return t.apply(i, n)
                    })
                }
            }, {
                key: "emitWithScope",
                value: function (t, i) {
                    for (var e = arguments.length, n = new Array(2 < e ? e - 2 : 0), a = 2; a < e; a++)
                        n[a - 2] = arguments[a];
                    this.handlers[t] && this.handlers[t].forEach(function (t) {
                        return t.apply(i, n)
                    })
                }
            }, {
                key: "addEventListener",
                value: function (t, i) {
                    this.handlers[t] || (this.handlers[t] = []),
                    this.handlers[t].push(i)
                }
            }, {
                key: "once",
                value: function (t, i) {
                    var e = this;
                    e.on(t, function n() {
                        i.apply(this, arguments),
                        e.removeListener(t, n)
                    })
                }
            }, {
                key: "removeEventListener",
                value: function (t, i) {
                    if (this.handlers[t]) {
                        var e = this.handlers[t].indexOf(i);
                        -1 < e && this.handlers[t].splice(e, 1)
                    }
                }
            }, {
                key: "removeAllListeners",
                value: function (t) {
                    this.handlers[t] = void 0
                }
            }]) && Vt(i.prototype, e),
            t
        }();
        e.d(i, "DEF_CHART", function () {
            return qt
        }),
        e.d(i, "DEF_DATA", function () {
            return Zt
        }),
        e.d(i, "PLUGINS", function () {
            return Jt
        }),
        e.d(i, "util", function () {
            return $t
        }),
        e.d(i, "createSingleChart", function () {
            return Qt
        }),
        e.d(i, "createMulChart", function () {
            return ti
        });
        var qt = n
          , Zt = lt
          , Jt = Gt
          , $t = {
              EV: Ut
          };
        function Qt(t) {
            var i = P(t)
              , e = new Lt(i)
              , n = new Nt(i)
              , a = new Kt;
            return e.initChart(a, n),
            e
        }
        function ti(t) {
            var i = P(t)
              , e = new Nt(i)
              , n = {};
            for (var a in t.charts) {
                var s = new Lt(i)
                  , o = new Kt;
                s.initChart(o, e),
                n[a] = s
            }
            return n
        }
    }
    ])
});
//# sourceMappingURL=clchart.js.map
