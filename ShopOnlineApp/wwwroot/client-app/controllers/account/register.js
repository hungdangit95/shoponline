var registerController = function () {
    this.initialize = function () {
        registerEvent();
    }

    function registerEvent() {


        //$("#btnCreate").on('click', function () {
        //    resetFormMaintainance();
        //    initTreeDropDownCategory();
        //    $('#modal-add-edit').modal('show');
        //});
        //$("#btnSelectImg").on('click',
        //    () => {
        //        $('#fileInputImage').click();
        //    });

        $("#fileImages").on('change', function () {
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
                    clearFileInput($("#fileImages"));
                    $('#avatar').val(path);
                    $("#image_place").attr("src", path);
                    $("#fileImages").val('');

                    // $('#image_place').append('<div class="col-md-3"><img width="200"  data-path="' + path + '" src="' + path + '">  <a class="btn  btn-sm  btn-deleted"><i class="fa fa-trash"></i></a> </div>  </div>');
                    shoponline.notify('Upload image succesfull!', 'success');
                },

                error: function () {
                    shoponline.notify('There was error uploading files!', 'error');
                }
            });
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