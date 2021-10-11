var ImageManagement = function () {
    var self = this;
    var index = 0;
    var parent = parent;

    var images = [];

    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-images', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $('#hidId').val(that);
            clearFileInput($("#fileImage"));
            loadImages();
            $('#modal-image-manage').modal('show');
        });
        $('body').on('click', '.btn-delete-image', function (e) {
            e.preventDefault();
            $(this).closest('div').remove();
        });
        $("#fileImage").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            $.ajax({
                type: "POST",
                url: "/Admin/Upload/UploadImage",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    clearFileInput($("#fileImage"));
                    images.push(path);
                    index++;
                    $('#image-list').append('<div class="col-md-3"><img width="100" data-id=' + index + ' data-path="' + path + '" src="' + path + '">  <a class="btn  btn-sm  btn-deleted" data-id='+index+'><i class="fa fa-trash"></i></a> </div>  </div>');
                    shoponline.notify('Đã tải ảnh lên thành công!', 'success');
                },
                error: function () {
                    shoponline.notify('There was error uploading files!', 'error');
                }
            });
        });


        $('body').on('click','.btn-deleted',
            function (e) {
                e.preventDefault();
                var id = $(this).data('id');
                $.each($('#image-list').find('img'), function (i, item) {
                    if (id === $(this).data('id')) {
                        $(this).closest('div').remove();
                    }
                });
                

            });

        $("#btnSaveImages").on('click', function () {
            var imageList = [];
            $.each($('#image-list').find('img'), function (i, item) {
                imageList.push($(this).data('path'));
            });
            $.ajax({
                url: '/admin/Product/SaveImages',
                data: {
                    productId: $('#hidId').val(),
                    images: imageList
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    $('#modal-image-manage').modal('hide');
                    $('#image-list').html('');
                    clearFileInput($("#fileImage"));
                }
            });
        });
    }
    function loadImages() {
        $.ajax({
            url: '/admin/Product/GetImages',
            data: {
                productId: $('#hidId').val()
            },
            type: 'get',
            dataType: 'json',
            success: function (response) {
                var render = '';
                $.each(response, function (i, item) {
                    render += '<div class="col-md-3"><img width="100" src="' +
                        item.Path +
                        '"> <a class="btn  btn-sm  btn-delete-image"><i class="fa fa-trash"></i></a></div>';
                });
                $('#image-list').html(render);
                clearFileInput($("#fileImage"));
            }
        });
    }

    function clearFileInput(ctrl) {
        try {
            ctrl.value = null;
        } catch (ex) { }
        if (ctrl.value) {
            ctrl.parentNode.replaceChild(ctrl.cloneNode(true), ctrl);
        }
    }
}