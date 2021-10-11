var blogCategoryController = function () {
    //this.currentId = 0;
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    function registerEvents() {
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtNameM: { required: true },
                txtOrderM: { number: true },
                txtHomeOrderM: { number: true }
            },
            message: {
                txtNameM: "Bạn phải nhập tên",
                txtOrderM: "Bạn phải nhập số thứ tự",
                txtHomeOrderM:"Bạn phải nhập"
            }
        });

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

        $('#btnCreate').off('click').on('click', function () {
            initTreeDropDownCategory();
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');
        });
        $('#btnSelectImg').on('click', function () {
            $('#fileInputImage').click();
        });
        $("#fileInputImage").on('change', function () {
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
                    $('#txtImage').val(path);
                    shoponline.notify('Upload image succesful!', 'success');

                },
                error: function () {
                    shoponline.notify('There was error uploading files!', 'error');
                }
            });
        });
        $('body').on('click', '#btnEdit', function (e) {
            e.preventDefault();

            var that = $('#hidIdM').val();
            $.ajax({
                type: "GET",
                url: "/Admin/BlogCategory/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    shoponline.startLoading();
                },
                success: function (response) {
                    
                    //console.log(response);
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#txtNameM').val(data.Name);
                    initTreeDropDownCategory(data.ParentId);

                    $('#txtDescM').val(data.Description);

                    //$('#txtImageM').val(data.ThumbnailImage);
                    $("#image_place").attr("src", data.Image);

                    $('#txtSeoKeywordM').val(data.SeoKeywords);
                    $('#txtSeoDescriptionM').val(data.SeoDescription);
                    $('#txtSeoPageTitleM').val(data.SeoPageTitle);
                    $('#txtSeoAliasM').val(data.SeoAlias);
                    
                    $('#ckStatusM').prop('checked', data.Status === 1);
                    $('#ckShowHomeM').prop('checked', data.HomeFlag);
                    $('#txtOrderM').val(data.SortOrder);
                    $('#txtHomeOrderM').val(data.HomeOrder);

                    $('#modal-add-edit').modal('show');
                    shoponline.stopLoading();
                },
                error: function (status) {
                    shoponline.notify('Có lỗi xảy ra', 'error');
                    shoponline.stopLoading();
                }
            });
        });

        $('body').on('click', '#btnDelete', function (e) {
            e.preventDefault();
            var that = $('#hidIdM').val();
            shoponline.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/BlogCategory/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        shoponline.startLoading();
                    },
                    success: function (response) {
                        shoponline.notify('Deleted success', 'success');
                        shoponline.stopLoading();
                        loadData();
                    },
                    error: function (status) {
                        shoponline.notify('Has an error in deleting progress', 'error');
                        shoponline.stopLoading();
                    }
                });
            });
        });

        function clearFileInput(ctrl) {
            try {
                ctrl.value = null;
            } catch (ex) { }
            if (ctrl.value) {
                ctrl.parentNode.replaceChild(ctrl.cloneNode(true), ctrl);
            }
        }

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = parseInt($('#hidIdM').val());
                var name = $('#txtNameM').val();
                var parentId = $('#ddlCategoryIdM').combotree('getValue');
                var description = $('#txtDescM').val();

                var image =  $("#image_place").attr("src");
                var order = parseInt($('#txtOrderM').val());
                var homeOrder = $('#txtHomeOrderM').val();

                var seoKeyword = $('#txtSeoKeywordM').val();
                var seoMetaDescription = $('#txtSeoDescriptionM').val();
                var seoPageTitle = $('#txtSeoPageTitleM').val();
                var seoAlias = $('#txtSeoAliasM').val();
                var status = $('#ckStatusM').prop('checked') === true ? 1 : 0;
                var showHome = $('#ckShowHomeM').prop('checked');

                $.ajax({
                    type: "POST",
                    url: "/Admin/BlogCategory/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        Description: description,
                        ParentId: parentId,
                        HomeOrder: homeOrder,
                        SortOrder: order,
                        HomeFlag: showHome,
                        Image: image,
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
                        shoponline.notify('Update success', 'success');
                        $('#modal-add-edit').modal('hide');

                        resetFormMaintainance();

                        shoponline.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        shoponline.notify('Has an error in update progress', 'error');
                        shoponline.stopLoading();
                    }
                });
            }
            return false;

        });
    }
    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        initTreeDropDownCategory('');

        $('#txtDescM').val('');
        $('#txtOrderM').val('');
        $('#txtHomeOrderM').val('');
        $('#txtImageM').val('');

        $('#txtMetakeywordM').val('');
        $('#txtMetaDescriptionM').val('');
        $('#txtSeoPageTitleM').val('');
        $('#txtSeoAliasM').val('');
        $('#txtSeoKeywordM').val('');
        $('#txtSeoDescriptionM').val('');

        $('#ckStatusM').prop('checked', true);
        $('#ckShowHomeM').prop('checked', false);
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
                if (selectedId !== undefined) {
                    $('#ddlCategoryIdM').combotree('setValue', selectedId);
                }
            }
        });
    }
    function loadData() {
        $.ajax({
            url: '/Admin/BlogCategory/GetAll',
            dataType: 'json',
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
                var treeArr = shoponline.unflattern(data);
                treeArr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });
                //var $tree = $('#treeBlogCategory');

                $('#treeBlogCategory').tree({
                    data: treeArr,
                    dnd: true,
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        // select the node
                        //$('#tt').tree('select', node.target);
                        //console.log($('#contentValue').html());
                        
                        $('#hidIdM').val(node.id);

                        //console.log($('#test').html());
                        // display context menu
                        $('#contextMenu').menu('show', {
                            left: e.pageX,
                            top: e.pageY
                        });
                    },
                    onDrop: function (target, source, point) {
                        //console.log(target);
                        //console.log(source);
                        //console.log(point);
                        var targetNode = $(this).tree('getNode', target);
                        if (point === 'append') {
                            var children = [];
                            $.each(targetNode.children, function (i, item) {
                                children.push({
                                    key: item.id,
                                    value: i
                                });
                            });

                            //Update to database
                            $.ajax({
                                url: '/Admin/BlogCategory/UpdateParentId',
                                type: 'post',
                                dataType: 'json',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id,
                                    items: children
                                },
                                success: function (res) {
                                    loadData();
                                }
                            });
                        }
                        else if (point === 'top' || point === 'bottom') {
                            $.ajax({
                                url: '/Admin/BlogCategory/ReOrder',
                                type: 'post',
                                dataType: 'json',
                                data: {
                                    sourceId: source.id,
                                    targetId: targetNode.id
                                },
                                success: function (res) {
                                    loadData();
                                }
                            });
                        }
                    }
                });

            }
        });
    }
}