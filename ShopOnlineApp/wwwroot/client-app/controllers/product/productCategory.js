var ProductCategoryController = function () {
    this.initialize = function () {
        filterRequest();
        registerEvents();
    }

    function registerEvents() {
        $(document).on('change', ".btnClick", function () {
            if ($(this).is(':checked')) {
                $(".btnClick").prop("checked", false);
                $(this).prop("checked", true);
            } else {
                $('this').prop('checked', false);

            }
        });
        $(document).on('change', ".linkColor", function () {
            if ($(this).is(':checked')) {
                $(".linkColor").prop("checked", false);
                $(this).prop("checked", true);
            } else {
                $('this').prop('checked', false);

            }
        });
        $(document).on('change', ".linkSize", function () {
            if ($(this).is(':checked')) {
                $(".linkSize").prop("checked", false);
                $(this).prop("checked", true);
            } else {
                $('this').prop('checked', false);

            }
        });



        var clickCategories = document.querySelectorAll('.btnClick');
        var i;
        for (i = 0; i < clickCategories.length; i++) {
            clickCategories[i].addEventListener('click', function (event) {
                setTimeout(() => {
                    $(document).on('change', ".btnClick", function () {
                        if ($(this).is(':checked')) {
                            $(".btnClick").prop("checked", false);
                            $(this).prop("checked", true);
                        } else {
                            $('this').prop('checked', false);
                        }
                    });
                    $(document).on('change', ".linkColor", function () {
                        if ($(this).is(':checked')) {
                            $(".linkColor").prop("checked", false);
                            $(this).prop("checked", true);
                        } else {
                            $('this').prop('checked', false);
                        }
                    });
                    $(document).on('change', ".linkSize", function () {
                        if ($(this).is(':checked')) {
                            $(".linkSize").prop("checked", false);
                            $(this).prop("checked", true);
                        } else {
                            $('this').prop('checked', false);

                        }
                    });

                    filterRequest(false);
                }, false);
            }, 200);
        }

        var clickColors = document.querySelectorAll('.linkColor');
        for (i = 0; i < clickColors.length; i++) {
            clickColors[i].addEventListener('click', function (event) {
                filterRequest(false);
            }, false);
        }
        var clickSizes = document.querySelectorAll('.linkSize');
        for (i = 0; i < clickSizes.length; i++) {
            clickSizes[i].addEventListener('click', function (event) {
                filterRequest(false);
            }, false);
        }

        $('#clickMe').on('click', function (e) {
            e.preventDefault();
            event.stopPropagation();
            event.stopImmediatePropagation();
            console.log($(this).val());

        });
        //$('#money').on('change',
        //    (e) => {
        //        console.log(e);
        //    });

        //$(document).on('change', "#pageSize", function () {
        //});
        $("#pageSize").change(function () {
            filterRequest(false);
        });


    }
    function filterRequest(isPageChanged) {
        var seoSelectItem = $("input[name='categories']:checked").val() ? $("input[name='categories']:checked").val() : "";
        var idSelectItem = $("input[name='categories']:checked").attr('data-id') ? $("input[name='categories']:checked").attr('data-id') : "";
        var sortBy = $("#sortBy option:selected").val();
        var pageSize = $('#pageSize option:selected').val();
        var color = $("input[name='color']:checked").val() ? $("input[name='color']:checked").val() : "";
        var size = $("input[name='size']:checked").val() ? $("input[name='size']:checked").val() : "";
        var seoAlias = $('#categoryCurrent').val();
        var currentCategoryId = $('#categoryCurrent').attr("data-id");
        var url = '';

        var template = $('#product-template').html();
        $.ajax({
            url: "loadData.html",
            type: 'Get',
            dataType: 'json',
            data: {
                id: idSelectItem ? idSelectItem : currentCategoryId,
                pageSize: pageSize,
                page: shoponline.configs.pageIndex,
                sortBy: sortBy,
                colorId: color,
                sizeId: size
            },
            success: function (response) {
                if (response) {
                    var render = "";
                    if (response.Items.length > 0) {
                        response.Items.map((item) => {
                            render += Mustache.render(template,
                                {
                                    Id: item.Id,
                                    Url: '/' + item.SeoAlias + '-p.' + item.Id + '.html',
                                    ProductName: item.Name,
                                    Image: item.Image,
                                    PromotionPrice: item.PromotionPrice,
                                    OriginalPrice: item.OriginalPrice,
                                    Description: item.Description
                                });
                            if (render !== "") {
                                $('#products-list').html(render);
                            }
                            wrapPaging(response.RowCount,
                                function () {
                                    filterRequest();
                                },
                                isPageChanged);
                        });
                    } else {

                        $('#lblTotalRecords').text(0);
                        $('#tbl-content').html("");
                        $('#products-list').html('');
                        var html = "<div class='alert alert-danger' role='alert'><b> Hiện không có bản ghi nào!</b>  </div >";
                        $('#products-list').html(html);
                        wrapPaging(0,
                            function () {
                                filterRequest();
                            },
                            isPageChanged);
                        $('#paginationUL').html('');
                    }
                }
            },
            error: (e) => {
                console.log(e);
            }
        });
        function wrapPaging(recordCount, callBack, changePageSize) {

            var totalsize = Math.ceil(recordCount / 5);
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
    }

}