var BaseController = function () {
    this.initialize = function() {
        registerEvents();
    }
    function registerEvents() {
        $('body').on('click',
            '.add-to-cart',
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
                    success: function(response) {
                        shoponline.notify(resources["AddCartOK"], 'success');
                        loadHeaderCart();
                    }
                });
            });

        $('body').on('click',
            '.remove-cart',
            function(e) {
                e.preventDefault();
                var id = $(this).data('id');
                $.ajax({
                    url: '/Cart/RemoveFromCart',
                    type: 'post',
                    data: {
                        productId: id
                    },
                    success: function(response) {
                        shoponline.notify(resources["RemoveCartOK"], 'success');
                        loadHeaderCart();
                        //loadMyCart();
                    }
                });
            });

        $('body').on('click',
            '.wish-list',
            function(e) {
                e.preventDefault();
                var id = $(this).data('id');
                $.ajax({
                    url: '/WishList/AddToWishList',
                    type: 'post',
                    data: {
                        productId: id
                    },
                    success: function() {
                        shoponline.notify('The product was added to wish list', 'success');
                        loadWishList();
                    }
                });
            });
    }

    function loadWishList() {
        $("#wishlist").load("/AjaxContent/WishList");
    }

    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart");

      

        //function loadMyCart() {
        //    $("#sidebarCart").load("/AjaxContent/MyCart");
        //}
    }
}