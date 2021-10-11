var slideController = function () {

    var obj = {
        groupAlias : [
            {
                value: "top",
                Id:"top"
            },
            {
                value: "brand",
                Id: "brand"
            }
        ]
    }

    this.initialize = function () {
        registerControl();
        $.when(loadGroupAlias())
            .done(function () {
                loadData();
            });
        //loadData();
        registerEvent();
    }
    function registerControl() {
        CKEDITOR.replace('txtContent', {});
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                            // CKEditor compatibility fix start.
                            && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };
    }
    function registerEvent() {

        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {
                txtNameM: { required: true },
                ddlCategoryIdM: { required: true },
                txtPriceM: {
                    required: true,
                    number: true
                }
            }
        });

        $('#ddlShowPage').on('change', function () {
            shoponline.configs.pageSize = $(this).val();
            shoponline.configs.pageIndex = 1;
            loadData(true);
        });
        $('#btnSearch').on('click',
            function () {
                loadData(true);
            });
        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                loadData();
            }
        });

        function clearFileInput(ctrl) {
            try {
                ctrl.value = null;
            } catch (ex) { }
            if (ctrl.value) {
                ctrl.parentNode.replaceChild(ctrl.cloneNode(true), ctrl);
            }
        }

        $("#btnCreate").on('click', function () {
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');

        });
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

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            loadDetails(that);
        });
        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            deleteProduct(that);
        });

        $('#btnSave').on('click', function (e) {
            saveSlide();
        });
    }
    function saveSlide() {
        if ($('#frmMaintainance').valid()) {
            var id = $('#hidIdM').val();
            var name = $('#txtNameM').val();
            //var categoryId = $('#ddlCategoryIdM').combotree('getValue');
            //var groupAlias = $('#ddlGroupAlias').val();

            var groupAlias = $("#ddlGroupAlias option:selected").text();
            var description = $('#txtDescM').val();
           // var image = $('#hidId').val();
            var image = $("#image_place").attr("src");
            var url = $('#txtURL').val();
            var content = CKEDITOR.instances.txtContent.getData();
            var status = $('#ckStatusM').prop('checked');
            $.ajax({
                type: "POST",
                url: "/Admin/Slide/SaveEntity",
                data: {
                    Id: id,
                    Name: name,
                    Image: image,
                    GroupAlias: groupAlias,
                    Url: url,
                    Description: description,
                    Content: content,
                    Status: status
                },
                dataType: "json",
                beforeSend: function () {
                    shoponline.startLoading();
                },
                success: function (response) {
                    shoponline.notify('Update Slide successful', 'success');
                    $('#modal-add-edit').modal('hide');
                    resetFormMaintainance();

                    shoponline.stopLoading();
                    loadData(true);
                },
                error: function () {
                    shoponline.notify('Has an error in save product progress', 'error');
                    shoponline.stopLoading();
                }
            });
            return false;
        }
    }

    function deleteProduct(id) {
        shoponline.confirm('Are you sure to delete?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Product/Delete",
                data: { id: id },
                dataType: "json",
                beforeSend: function () {
                    shoponline.startLoading();
                },
                success: function (response) {
                    shoponline.notify('Delete successful', 'success');
                    shoponline.stopLoading();
                    loadData();
                },
                error: function (status) {
                    shoponline.notify('Has an error in delete progress', 'error');
                    shoponline.stopLoading();
                }
            });
        });
    }
    function loadDetails(id) {
        $.ajax({
            type: "GET",
            url: "/Admin/Slide/GetById",
            data: { id: id },
            dataType: "json",
            beforeSend: function () {
                shoponline.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#hidIdM').val(data.Id);
                $('#txtNameM').val(data.Name);

                $('#txtDescM').val(data.Description);
                loadGroupAlias(data.GroupAlias);
                $("#image_place").attr("src", data.Image);
                 //$('#txtImageM').val(data.ThumbnailImage);
                $('#txtURL').val(data.Url);
                // get instance by id 
                CKEDITOR.instances.txtContent.setData(data.Content);
                $('#ckStatusM').prop('checked', data.Status === true);
                $('#modal-add-edit').modal('show');
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

        $('#txtDescM').val('');
        $('#txtPriceM').val('0');
        $('#txtURL').val('');
        //CKEDITOR.instances.txtContentM.setData('');
        $('#ckStatusM').prop('checked', true);
    }

    function loadData(isPageChanged) {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            url: '/Admin/Slide/GetAllPaging',
            data: {
                //categoryId: $('#ddlCategorySearch').val(),
                searchText: $('#txtKeyword').val(),
                pageIndex: shoponline.configs.pageIndex,
                pageSize: shoponline.configs.pageSize
            },
            dataType: 'json',
            success: (response) => {
                if (response.Data.Items.length > 0) {
                    $.each(response.Data.Items, 
                        function (i, item) {

                            render += Mustache.render(template,
                                {
                                    Id: item.Id,
                                    Name: item.Name,
                                    Description: item.Description,
                                    Image: item.Image === null
                                        ? '<img  src="/admin-side/images/user.png" width=90'
                                        : '<img src="' + item.Image + '" width=90 />',
                                    GroupAlias: item.GroupAlias,
                                    Url: item.Url,
                                    Status: item.Status === true ? shoponline.getStatus(1) : shoponline.getStatus(0)
                        });
                            $('#lblTotalRecords').text(response.Data.RowCount);
                            if (render !== "") {
                                $('#tbl-content').html(render);
                            }
                            wrapPaging(response.Data.RowCount,
                                function () {
                                    loadData();
                                },
                                isPageChanged);
                        });
                } else {
                    $('#lblTotalRecords').text(0);
                    $('#tbl-content').html("");
                    wrapPaging(0,
                        function () {
                            loadData();
                        },
                        isPageChanged);
                }

            },
            error: function (status) {
                shoponline.notify('Không thể load dữ liệu', 'lỗi');
            }
        });
        function wrapPaging(recordCount, callBack, changePageSize) {
            var totalsize = Math.ceil(recordCount / shoponline.configs.pageSize);
            //Unbind pagination if it existed or click change pagesize
            if ($('#paginationUL a').length === 0 || changePageSize === true) {
                $('#paginationUL').empty();
                $('#paginationUL').removeData("twbs-pagination");
                $('#paginationUL').unbind("page");
            }
            //Bind Pagination Event
            $('#paginationUL').twbsPagination({
                totalPages: totalsize,
                visiblePages: 7,
                first: 'Đầu',
                prev: 'Trước',
                next: 'Tiếp',
                last: 'Cuối',
                onPageClick: function (event, p) {
                    shoponline.configs.pageIndex = p;
                    setTimeout(callBack(), 200);
                }
            });
        }

    };

    function loadGroupAlias(val) {
        var render = "";
        $.each(obj.groupAlias, function (i, item) {
            if (item.value === val) {
                render += "<option selected='select' value='" + item.id + "'>" + item.value + "</option>";
            } else {
                render += "<option value='" + item.id + "'>" + item.value + "</option>";
            }
        });
        $('#ddlGroupAlias').html(render);
    }
}