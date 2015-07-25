function InitCenter() {
    try {
        var LeftColumn = document.getElementById("LeftColumn");
        var CenterColumn = document.getElementById("CenterColumn");
        var mw = 400;
    } catch (err) { alert(err); }

    try {
        if (CenterColumn != null) {
            var myWidth = 0, myHeight = 0;
            if (typeof (window.innerWidth) == 'number') {
                //Non-IE
                myWidth = window.innerWidth;
                myHeight = window.innerHeight;
            }
            else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
                //IE 6+ in 'standards compliant mode'
                myWidth = document.documentElement.clientWidth;
                myHeight = document.documentElement.clientHeight;
            }
            else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
                //IE 4 compatible
                myWidth = document.body.clientWidth;
                myHeight = document.body.clientHeight;
            }

            var LeftOffset = LeftColumn.offsetWidth;

            if (CenterColumn.offsetLeft + CenterColumn.offsetWidth < LeftOffset + mw) {
                CenterColumn.style.right = myWidth - (LeftOffset + mw + 14) + "px";
            }
            else {
                CenterColumn.style.right = "5px";
            }

            var ccw = CenterColumn.offsetWidth;

            var w = 0;
            for (var i = 0; i < CenterColumn.getElementsByTagName("DIV").length; i++) {
                var el = CenterColumn.getElementsByTagName("DIV").item(i);
                if (el.offsetWidth > w) { w = el.offsetWidth; }
            }
            var idw = w;

            w = 0;
            for (var i = 0; i < CenterColumn.getElementsByTagName("TABLE").length; i++) {
                var el = CenterColumn.getElementsByTagName("TABLE").item(i);
                if (el.offsetWidth > w) { w = el.offsetWidth; }
            }
            var itw = w;

            w = 0;
            for (var i = 0; i < CenterColumn.getElementsByTagName("IMG").length; i++) {
                var el = CenterColumn.getElementsByTagName("IMG").item(i);
                if (el.offsetWidth > w) { w = el.offsetWidth; }
            }
            var iiw = w;
            ccw = ccw < idw ? idw : ccw;
            ccw = ccw < itw ? itw : ccw;
            ccw = ccw < iiw ? iiw : ccw;
            ccw = ccw < mw ? mw : ccw;
            ccw += 10;

            CenterColumn.style.right = "2px";

            if (ccw > CenterColumn.offsetWidth) {
                var LeftPosition = CenterColumn.offsetLeft;
                CenterColumn.style.right = ((myWidth - (LeftPosition + ccw))) + "px";
            }
        }
    }
    catch (err) {
        alert(err.description);
    }
}