var common = {
    init: function () {
        common.registerEvents();
    },
    registerEvents: function () {
        $("#txtKeyword").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Product/SuggestSearch",
                    dataType: "json",
                    data: {
                        keyword: request.term
                    },
                    success: function (res) {
                        response(res);
                    }
                });
            },
            focus: function (event, ui) {
                $("#txtKeyword").val(ui.item.Name);
                return false;
            },
            select: function (event, ui) {
                $("#txtKeyword").val(ui.item.Name);
                return false;
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<div style='float:left'> <img height='40px;' width='40px;' style='padding-right:7px;'  src=" + item.Image + " alt=" + item.Name + "/> </div>")
                .append("<div style='float:left'>")
                .append("<span style='font-weight:bold;'>" + item.Name + "</span>")
                .append("<p  style='color:#f3770e;font-style: italic;'>" + shoponline.formatNumber(item.Price, 0) + "/VND" + "</p> ")
                .append("</div>")
                .appendTo(ul);
        };
        
    }
}
common.init();