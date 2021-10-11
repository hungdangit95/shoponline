var accountController = function () {
    this.initialize = function () {
        loadDetails();
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
                    $('#hidId').val(path);
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
        
        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            deleteBlog(that);
        });

       
        $('#btnSave').on('click', function (e) {
            saveBlog();
        });
    }
    function saveBlog() {
        if ($('#frmMaintainance').valid()) {
            var id = $('#hidIdM').val();
            var name = $('#txtNameM').val();
            var categoryId = $('#ddlCategoryIdM').combotree('getValue');
            var description = $('#txtDescM').val();
            //var image = $('#image_place').val();
            var image = $("#image_place").attr("src");
            var tags = $('#txtTagM').val();
            var seoKeyword = $('#txtMetakeywordM').val();
            var seoMetaDescription = $('#txtMetaDescriptionM').val();
            var seoPageTitle = $('#txtSeoPageTitleM').val();
            var seoAlias = $('#txtSeoAliasM').val();
            var content = CKEDITOR.instances.txtContent.getData();
            var status = $('#ckStatusM').prop('checked') === true ? 1 : 0;
            var hot = $('#ckHotM').prop('checked');
            var showHome = $('#ckShowHomeM').prop('checked');

            $.ajax({
                type: "POST",
                url: "/Admin/Blog/SaveEntity",
                data: {
                    Id: id,
                    Name: name,
                    blogCategoryId: categoryId,
                    Image: image,
                    Description: description,
                    Content: content,
                    HomeFlag: showHome,
                    HotFlag: hot,
                    Tags: tags,
                    Status: status,
                    SeoPageTitle: seoPageTitle,
                    SeoAlias: seoAlias,
                    SeoKeywords: seoKeyword,
                    SeoDescription: seoMetaDescription
                },
                dataType: "json",
                beforeSend: function () {
                    shoponline.startLoading();
                },
                success: function (response) {
                    shoponline.notify('Update Blog successful', 'success');
                    $('#modal-add-edit').modal('hide');
                    resetFormMaintainance();

                    shoponline.stopLoading();
                    loadData(true);
                },
                error: function () {
                    shoponline.notify('Has an error in save Blog progress', 'error');
                    shoponline.stopLoading();
                }
            });
            return false;
        }
    }
    function loadDetails() {
        $.ajax({
            type: "GET",
            url: "/Account/GetInformationUser",
            dataType: "json",
            beforeSend: function () {
                shoponline.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#hidIdM').val(data.Id);
                $('#email').val(data.Email);
                if (data.PhoneNumber) {
                    $('#phone').val(data.PhoneNumber);
                }
                $('#name').val(data.FullName);
                if (data.Avatar) {
                    $("#image_place").attr("src", data.Avatar);
                }
                //$('#txtImageM').val(data.ThumbnailImage);
                //$('#txtMetakeywordM').val(data.SeoKeywords);
               
                // get instance by id 
                //if (data.Gender) {
                //    $('#ckNu').prop('checked', data.Status === 1);
                //    $('#ckNam').prop('checked', data.Status === 1);
                //}
                shoponline.stopLoading();
            },
            error: function (status) {
                shoponline.notify('Có lỗi xảy ra', 'error');
                shoponline.stopLoading();
            }
        });
    }
    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        initTreeDropDownCategory('');

        $('#txtDescM').val('');

        //$('#txtImageM').val('');

        $('#txtTagM').val('');
        $('#txtMetakeywordM').val('');
        $('#txtMetaDescriptionM').val('');
        $('#txtSeoPageTitleM').val('');
        $('#txtSeoAliasM').val('');

        //CKEDITOR.instances.txtContentM.setData('');
        $('#ckStatusM').prop('checked', true);
        $('#ckHotM').prop('checked', false);
        $('#ckShowHomeM').prop('checked', false);
    }

}