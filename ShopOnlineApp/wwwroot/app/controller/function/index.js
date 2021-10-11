var functionController = function () {
    //this.currentId = 0;
    var propertyFunction = {
        isUpdated: false
    }
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
                txtHomeOrderM: { number: true },
                txtURL: { required: true },
                txtIconCss: {required:true}
            },
            message: {
                txtNameM: "Bạn phải nhập tên",
                txtOrderM: "Bạn phải nhập số thứ tự",
                txtHomeOrderM: "Bạn phải nhập",
                txtURL: "Bạn phải nhập đường dẫn",
                txtIconCss:"Bạn phải nhập Icon"
            }
        });
        $('#btnCreate').off('click').on('click', function () {
            initTreeDropDownFunction();
            resetFormMaintainance();
            propertyFunction.isUpdated = false;
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
            propertyFunction.isUpdated = true;
            $.ajax({
                type: "GET",
                url: "/Admin/Function/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    shoponline.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#txtId').val(data.Id);
                    $('#txtNameM').val(data.Name);
                    initTreeDropDownFunction(data.ParentId);
                    $('#txtId').prop('disabled', true);
                    $('#txtURL').val(data.URL);
                    $('#txtIconCss').val(data.IconCss);
                    $('#ckStatusM').prop('checked', data.Status === 1);
                    $('#txtOrderM').val(data.SortOrder);
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
                    url: "/Admin/Function/Delete",
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

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#txtId').val();
                var name = $('#txtNameM').val();
                var parentId = $('#ddlFunctionIdM').combotree('getValue');
                var order = parseInt($('#txtOrderM').val());
                var iconCss = $('#txtIconCss').val();
                var url =  $('#txtURL').val();
                var status = $('#ckStatusM').prop('checked') === true ? 1 : 0;

                $.ajax({
                    type: "POST",
                    url: "/Admin/Function/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        ParentId: parentId,
                        IconCss: iconCss,
                        SortOrder: order,
                        Status: status,
                        IsUpdated: propertyFunction.isUpdated,
                        URL: url
                    },
                    dataType: "json",
                    beforeSend: function () {
                        shoponline.startLoading();
                    },
                    success: function (response) {
                        shoponline.notify('success', 'success');
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
        $('#hidIdM').val('');
        $('#txtId').val('');
        $('#txtNameM').val('');
        initTreeDropDownFunction();
        $('#txtId').prop('disabled', false);
        $('#txtURL').val('');
        $('#txtIconCss').val('');
        //$('#ckStatusM').prop('checked', data.Status === 1);
        $('#txtOrderM').val('');

        $('#ckStatusM').prop('checked', false);
        
    }
    function initTreeDropDownFunction(selectedId) {
        $.ajax({
            url: "/Admin/Function/GetAll",
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
                $('#ddlFunctionIdM').combotree({
                    data: arr
                });
                if (selectedId !== undefined) {
                    $('#ddlFunctionIdM').combotree('setValue', selectedId);
                }
            }
        });
    }
    function loadData() {
        $.ajax({
            url: '/Admin/Function/GetAll',
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
                //var $tree = $('#treeProductCategory');

                $('#treeFunction').tree({
                    data: treeArr,
                    dnd: true,
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        // select the node
                        //$('#tt').tree('select', node.target);
                        //console.log($('#contentValue').html());

                        $('#hidIdM').val(node.id);
                        // display context menu
                        $('#contextMenu').menu('show', {
                            left: e.pageX,
                            top: e.pageY
                        });
                    },
                    onDrop: function (target, source, point) {
                        console.log(target);
                        console.log(source);
                        console.log(point);
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
                                url: '/Admin/Function/UpdateParentId',
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
                                url: '/Admin/Function/ReOrder',
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