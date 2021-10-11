var ProductDetailController = function () {
    this.initialize = function () {
        registerEvents();
    }
   
    function registerEvents() {
        $('body').on('click','#btnAddToCart', function (e) {
            e.preventDefault();
            var id = parseInt($(this).data('id'));
            var colorId = parseInt($('#ddlColorId').val());
            var sizeId = parseInt($('#ddlSizeId').val());
            $.ajax({
                url: '/Cart/AddToCart',
                type: 'post',
                dataType: 'json',
                data: {
                    productId: id,
                    quantity: (parseInt($('#txtQuantity').val()) > 0 ? parseInt($('#txtQuantity').val()):1),
                    color: colorId,
                    size: sizeId
                },
                success: function () {
                    shoponline.notify('Product was added successful', 'success');
                    loadHeaderCart();
                }
            });
        });
    }
    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart");
    }
}