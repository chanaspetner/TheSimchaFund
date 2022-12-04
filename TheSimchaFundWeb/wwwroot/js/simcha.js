$(() => {
    $("#new-simcha").on('click', function () {
        $(".modal").modal();
    })
    $(".btn-secondary").on('click', function () {
        $(".modal").modal('hide');
    })
})
