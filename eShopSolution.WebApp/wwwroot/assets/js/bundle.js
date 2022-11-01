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

//handle layout (closure code)
var siteHandle = (function () {
    loadMessenger = function () {
        var chatbox = document.getElementById('fb-customer-chat');
        chatbox.setAttribute("page_id", "111298008427085");
        chatbox.setAttribute("attribution", "biz_inbox");

        window.fbAsyncInit = function () {
            FB.init({
                xfbml: true,
                version: 'v15.0'
            });
        };

        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = 'https://connect.facebook.net/vi_VN/sdk/xfbml.customerchat.js';
            js.defer = true;
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));
    };

    init = function () {
        //load css async
        //k12Utils.loadStyleSheet('https://fonts.googleapis.com/css2?family=Montserrat:wght@400;600&display=swap');
        //k12Utils.loadStyleSheet('assets/fonts/UTM-Hanzel.ttf');
        //k12Utils.loadStyleSheet('https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css');

        //k12Utils.loadStyleSheet('assets/css/site.css');
        //k12Utils.loadStyleSheet('assets/pages/home/css/layout.css');

        loadMessenger();
    };

    return {
        init: init
    }
})();

//tuần tự code
//bắt đầu chạy code khi site đã sẵn sàng
window.onload = function () {
    siteHandle.init();
}
