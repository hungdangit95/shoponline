var connection = new signalR.HubConnectionBuilder()
    .withUrl("/onlineShopHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();
connection.start().catch(err => console.error(err.toString()));

connection.on("ReceiveMessage", (message) => {
    var template = $('#announcement-template').html();
    var html = Mustache.render(template, {
        Content: message.content,
        Id: message.id,
        Title: message.title,
        FullName: message.fullName,
        Avatar: message.avatar
    });
    $.ajax({
        type: "POST",
        url: "/Admin/Announcement/CreateAnnoucement",
        data: {
            Content: message.content,
            Id: message.id,
            Title: message.title,
            UserId: message.userId
        },
        dataType: "json",
        beforeSend: function () {
            shoponline.startLoading();
        },
        success: function (response) {

        },
        error: function (status) {
            console.log(status);
        }
    });

    $('#announcementList').prepend(html);
    var data = $('#totalAnnouncement').text();
    var totalAnnounce = 0;
    if (data) {
         totalAnnounce = parseInt($('#totalAnnouncement').text()) + 1;
    } else {
         totalAnnounce =  1;
    }
    
    $('#totalAnnouncement').text(totalAnnounce);
});