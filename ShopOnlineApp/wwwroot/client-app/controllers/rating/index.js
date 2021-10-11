var RateController = function () {
    
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('#addReview').on('click', function (e) {
            e.preventDefault();
            var quantity = $("input[name='quantity']:checked").val();
            var price = $("input[name='price']:checked").val();
            var value = $("input[name='value']:checked").val();
            var nickname = $('#nickname').val();
            var review = $('#review').val();
            var summary = $('#summary').val();
            
            $.ajax({
                url: '/Rating/Rating',
                type: 'post',
                dataType: 'json',
                data: {
                    quantity: quantity,
                    price: price,
                    value: value,
                    nickname: nickname,
                    review: review,
                    summary: summary
                },
                success: function () {
                    shoponline.notify('Rating successfull', 'success');
                    resetForm();
                    loadRating();
                }
            });
        });
    }
    
    function loadRating() {
        $("#resetRating").load("/AjaxContent/Rating");
    }

    function resetForm() {
        $('#nickname').val('');
        $('#review').val('');
        $('#summary').val('');
        $('input[name="quantity"]').prop('checked', false);
        $('input[name="price"]').prop('checked', false);
        $('input[name="value"]').prop('checked', false);
    }
    
}