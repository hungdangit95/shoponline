var blogController = function () {
    
    this.initialize = function () {
        loadDataCategory();
        loadData();
        registerEvent();
        registerControl();
      
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
            initTreeDropDownCategory();
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

        $('#btnImportExcel').on('click', function () {
            var fileUpload = $("#fileInputExcel").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();
            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append("files", files[i]);
            }
            // Adding one more key to FormData object  
            fileData.append('categoryId', $('#ddlCategoryIdImportExcel').combotree('getValue'));
            $.ajax({
                url: '/Admin/Blog/ImportExcel',
                type: 'POST',
                data: fileData,
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                success: function (data) {
                    $('#modal-import-excel').modal('hide');
                    loadData();

                }
            });
            return false;
        });

        $('#btn-export').on('click', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Blog/ExportExcel",
                beforeSend: function () {
                    shoponline.startLoading();
                },
                success: function (response) {
                    window.location.href = response;
                    shoponline.stopLoading();
                },
                error: function () {
                    shoponline.notify('Has an error in progress', 'error');
                    shoponline.stopLoading();
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
            deleteBlog(that);
        });

        $('#btn-import').on('click', function () {
            initTreeDropDownCategory();
            $('#modal-import-excel').modal('show');
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

    function deleteBlog(id) {
        shoponline.confirm('Are you sure to delete?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Blog/Delete",
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
            url: "/Admin/Blog/GetById",
            data: { id: id },
            dataType: "json",
            beforeSend: function () {
                shoponline.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#hidIdM').val(data.Id);
                $('#txtNameM').val(data.Name);
                initTreeDropDownCategory(data.BlogCategoryId);
                $('#txtDescM').val(data.Description);
                $("#image_place").attr("src", data.Image);
                 //$('#txtImageM').val(data.ThumbnailImage);
                $('#txtTagM').val(data.Tags);
                $('#txtMetakeywordM').val(data.SeoKeywords);
                $('#txtMetaDescriptionM').val(data.SeoDescription);
                $('#txtSeoPageTitleM').val(data.SeoPageTitle);
                $('#txtSeoAliasM').val(data.SeoAlias);
                // get instance by id 
                CKEDITOR.instances.txtContent.setData(data.Content);
                $('#ckStatusM').prop('checked', data.Status === 1);
                $('#ckHotM').prop('checked', data.HotFlag);
                $('#ckShowHomeM').prop('checked', data.HomeFlag);
                $('#modal-add-edit').modal('show');
                shoponline.stopLoading();
            },
            error: function (status) {
                shoponline.notify('Có lỗi xảy ra', 'error');
                shoponline.stopLoading();
            }
        });
    }

    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            url: "/Admin/BlogCategory/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.Id,
                        text: item.Name,
                        parentId: item.ParentId,
                        sortOrder: item.SortOrder
                    });
                });
                var arr = shoponline.unflattern(data);
                $('#ddlCategoryIdM').combotree({
                    data: arr
                });

                $('#ddlCategoryIdImportExcel').combotree({
                    data: arr
                });
                if (selectedId !== undefined) {
                    $('#ddlCategoryIdM').combotree('setValue', selectedId);
                }
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
    function loadDataCategory() {
        $.ajax({
            type: 'GET',
            url: '/Admin/BlogCategory/GetAll',
            dataType: 'json',
            success: (response) => {
                var result = "<option value=''>---Chọn danh mục---</option>";
                $.each(response,
                    function (i, item) {
                        var temp = Object.assign({}, item);
                        result += "<option value='" + temp.Id + "'>" + temp.Name + "</option>";
                    });
                $('#ddlCategorySearch').html(result);
            },
            error: function (status) {
                shoponline.notify('Không thể load danh mục sản phẩm ', 'lỗi');
            }
        });
    }
    function loadData(isPageChanged) {
        var template = $('#table-template').html();
        var render = "";
        $.ajax({
            type: 'GET',
            url: '/Admin/Blog/GetAllPaging',
            data: {
                categoryId: $('#ddlCategorySearch').val(),
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
                                    Image: item.Image === null
                                        ? '<img src="/admin-side/images/user.png" width=50'
                                        : '<img src="' + item.Image + '" width=50 />',
                                    CategoryName: item.BlogCategory.Name,
                                    CreatedDate: shoponline.dateTimeFormatJson(item.DateModified),
                                    Status: shoponline.getStatus(item.Status)
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

}