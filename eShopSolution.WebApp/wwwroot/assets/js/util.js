//tiện ích của toàn bộ site
var shopUtils = (function () {
    loadStyleSheet = function (src) {
        if (document.createStyleSheet) {
            document.createStyleSheet(src);
        }
        else {
            let linkEl = document.createElement('link');
            linkEl.setAttribute('rel', 'stylesheet');
            linkEl.setAttribute('href', src);
            document.querySelector("head").append(linkEl);
        }
    };

    return {
        loadStyleSheet: loadStyleSheet
    }
})();
