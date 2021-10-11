var BaseController = function () {

    this.initialize = function () {
        loadAnnouncement();
        registerEvents();
    }

    function registerEvents() {
        //$('body').on('click',
        //    '.btn-edit',
        //    function(e) {
        //        e.preventDefault();
        //        var that = $(this).data('id');
        //        $.ajax({
        //            type: "GET",
        //            url: "/Admin/Role/GetById",
        //            data: { id: that },
        //            dataType: "json",
        //            beforeSend: function() {
        //                shoponline.startLoading();
        //            },
        //            success: function(response) {
        //                var data = response;
        //                $('#hidId').val(data.Id);
        //                $('#txtName').val(data.Name);
        //                $('#txtDescription').val(data.Description);
        //                $('#modal-add-edit').modal('show');
        //                shoponline.stopLoading();
        //            },
        //            error: function(status) {
        //                shoponline.notify('Có lỗi xảy ra', 'error');
        //                shoponline.stopLoading();
        //            }
        //        });
        //    });

        $('body').on('click',
            '.read',
            function(e) {
                e.preventDefault();
                var that = $(this).data('id');

                $.ajax({
                    type: "Post",
                    url: "/Admin/Announcement/MarkAsRead",
                    data: {
                         id: that 
                    },
                    dataType: "json",
                    beforeSend: function() {
                        shoponline.startLoading();
                    },
                    success: function(response) {
                        loadAnnouncement();
                    }
                });
            });
    }

    function loadAnnouncement() {
        $.ajax({
            type: "GET",
            url: "/Admin/Announcement/GetAllPaging",
            data: {
                page: shoponline.configs.pageIndex,
                pageSize: shoponline.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                shoponline.startLoading();
            },
            success: function (response) {
                var template = $('#announcement-template').html();
                var render = "";
                if (response.RowCount > 0) {
                    //$('#announcementArea').show();
                    $.each(response.Results, function (i, item) {
                        render += Mustache.render(template, {
                            Content: item.Content,
                            Id: item.Id,
                            Title: item.Title,
                            DateCreated: shoponline.dateTimeFormatJson(item.DateCreated)
                        });
                    });
                    render += $('#announcement-tag-template').html();
                    $("#totalAnnouncement").text(response.RowCount);
                    if (render != undefined) {
                        $('#announcementList').html(render);
                    }
                }
                else {
                    $("#totalAnnouncement").text(null);
                    $('#annoncementList').html('');
                }
                shoponline.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };

  
    
}