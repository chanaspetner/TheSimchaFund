$(() => {
    $(".table").on('click', '.edit-person', function () {
        
        const id = $(this).data('id');
        const firstName = $(this).data('first-name');
        const lastName = $(this).data('last-name');
        const phoneNumber = $(this).data('phone-number');
        const alwaysInclude = $(this).data('alwaysInclude');
        const date = $(this).data('date');

        console.log(date);
        $("#contributor_first_name").attr('value', `${firstName}`);
        $("#contributor_last_name").attr('value', `${lastName}`);
        $("#contributor_phone_number").attr('value', `${phoneNumber}`);
        $("#contributor_created_at").attr('value', `${date}`);
        $("#contributor_always_include").prop('checked', alwaysInclude === 'True');
        $("#contrib-id").attr('value', `${id}`);
        $("#initialDepositDiv").remove();
        $("#post-action").attr('action', '/contributor/edit');
        $(".new-contrib").modal();

    });


    $("#new-contributor").on('click', function () {
        $(".new-contrib").modal();
    });

    $(".btn-secondary").on('click', function () {
        $("#edit").attr('value', 'false')
        $(".modal").modal("hide");
    });

    $(".table").on('click', '.deposit-button', function () {
        const id = $(this).data('id');
        const firstName = $(this).data('first-name');
        const lastName = $(this).data('last-name');

        $("#deposit-name").append(`${firstName} ${lastName}`);
        $("#personId").attr('value', `${id}`);
        $(".deposit").modal();
    });
})
