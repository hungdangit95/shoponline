var wishListController = function () {
    
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            $.ajax({
                url: '/WishList/RemoveFromWishList',
                type: 'post',
                data: {
                    productId: id
                },
                success: function () {
                    shoponline.notify('Removing wish list is successful.', 'success');
                    loadWishList();
                    loadData();
                }
            });
        });

        $('body').on('click','.btn-add',
            function(e) {
                e.preventDefault();
                var id = $(this).data('id');
                $.ajax({
                    url: '/Cart/AddToCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: 1,
                        color: 0,
                        size: 0
                    },
                    success: function (response) {
                        shoponline.notify('The product was added to cart', 'success');
                        loadHeaderCart();
                    }
                });
            });
    }

    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart");
    }

    function loadWishList() {
        $("#wishlist").load("/AjaxContent/WishList");
    }
    
    function loadData() {
        $.ajax({
            url: '/WishList/GetWishList',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var productIds = "";
                var template = $('#template-cart').html();
                var render = "";
                $.each(response, function (i, item) {

                    render += Mustache.render(template,
                        {
                            ProductId: item.Product.Id,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: shoponline.formatNumber(item.Product.Price, 0),
                            Url: '/' + item.Product.SeoAlias + "-p." + item.Product.Id + ".html"
                        });
                });
                if (render !== "")
                    $('#table-cart-content').html(render);
                else
                    $('#table-cart-content').html('You have no wish list in cart');
            }
        });
        return false;
    }
}