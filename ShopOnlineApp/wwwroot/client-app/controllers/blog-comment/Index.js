var BlogCommentController = function () {

    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('#comment').on('click', function (e) {
            e.preventDefault();
            var name = $('#name').val();
            var email = $('#email').val();
            var message = $('#message').val();

            $.ajax({
                url: '/BlogComment/Comment',
                type: 'post',
                dataType: 'json',
                data: {
                    name: name,
                    email: email,
                    message: message
                },
                success: function () {
                    shoponline.notify('Comment successfull', 'success');
                    resetForm();
                    loadRating();
                }
            });
        });
    }

    function loadRating() {
        $("#resetRating").load("/AjaxContent/BlogComment");
    }

    function resetForm() {
       $('#name').val('');
       $('#email').val('');
       $('#message').val('');
    }

}