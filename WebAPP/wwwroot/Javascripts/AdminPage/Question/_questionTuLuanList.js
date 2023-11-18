$(document).ready(function () {
    async function showScoreInputDialogTuLuan(
        id,
        intCategoryId,
        remainScore,
        thisScore
    ) {
        const { value: soDiem } = await Swal.fire({
            title: "Chọn điểm số cho câu hỏi",
            icon: "question",
            input: "range",
            inputLabel: "Số điểm chưa gán còn lại: " + remainScore + " điểm",
            inputAttributes: {
                min: "0",
                max: "10",
                step: "0.25",
            },
            inputValue: parseFloat(thisScore),
        });
        var diemSoSanh = parseFloat(remainScore) + parseFloat(thisScore);
        if (soDiem <= diemSoSanh) {
            $.ajax({
                url: "UpdateScoreTuluan",
                type: "POST",
                data: {
                    id: id,
                    Score: soDiem,
                },
                success: function (result) {
                    if (result.success) {
                        Swal.fire({
                            icon: "success",
                            title: "Sửa điểm số thành công",
                            showConfirmButton: false,
                            timer: 1500,
                        });
                        $.ajax({
                            url: "GetAllTuLuanByCategoryId",
                            type: "POST",
                            data: {
                                id: intCategoryId,
                            },
                            success: function (result) {
                                $("#listQuestionTuLuan").html(result);
                            },
                        });
                    }
                },
            });
        }
        if (soDiem > diemSoSanh) {
            Swal.fire(
                "Điểm số không phù hợp",
                "Bạn chỉ được gán thêm tối đa: " + remainScore + " điểm",
                "error"
            );
        }
    }

    $(".btnSuaDiemTuLuan").click(function () {
        var categoryId = $(this).data("abc");
        var intCategoryId = parseInt(categoryId);
        var id = $(this).data("id");
        var thisScore = $(this).data("this-score");
        $.ajax({
            url: "GetTotalScore",
            type: "POST",
            data: {
                categoryId: intCategoryId,
            },
            success: function (result) {
                if (result.success) {
                    let nowTotalScore = result.result;
                    let remainScore = 10 - nowTotalScore;
                    showScoreInputDialogTuLuan(
                        id,
                        intCategoryId,
                        remainScore,
                        thisScore
                    );
                } else {
                    Swal.fire(
                        "Lỗi máy chủ",
                        "Thao tác đã được huỷ bỏ!",
                        "error"
                    );
                }
            },
            error: function (xhr, status, error) {
                Swal.fire("Lỗi máy chủ", "Thao tác đã được huỷ bỏ!", "error");
            },
        });
    });

    $(".btnXoaCauHoiTuLuan").click(function () {
        var id = $(this).data("id");
        var categoryId = $(this).data("abc");
        var intCategoryId = parseInt(categoryId);

        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: "btn btn-success",
                cancelButton: "btn btn-danger",
            },
            buttonsStyling: true,
        });
        swalWithBootstrapButtons
            .fire({
                title: "Xoá câu hỏi?",
                text: "Dữ liệu liên quan tới câu hỏi sẽ bị xoá",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Xác nhận xoá",
                cancelButtonText: "Huỷ bỏ",
                reverseButtons: true,
            })
            .then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: "DeleteTuLuan",
                        type: "POST",
                        data: {
                            id: id,
                        },
                        success: function (result) {
                            if (result.success) {
                                Swal.fire({
                                    icon: "success",
                                    title: "Xoá câu hỏi thành công",
                                    showConfirmButton: false,
                                    timer: 1500,
                                });
                                $.ajax({
                                    url: "GetAllTuLuanByCategoryId",
                                    type: "POST",
                                    data: {
                                        id: intCategoryId,
                                    },
                                    success: function (result) {
                                        $("#listQuestionTuLuan").html(result);
                                    },
                                });
                            } else {
                                Swal.fire(
                                    "Lỗi máy chủ",
                                    "Thao tác đã được huỷ bỏ!",
                                    "error"
                                );
                            }
                        },
                        error: function (xhr, status, error) {
                            Swal.fire(
                                "Lỗi máy chủ",
                                "Thao tác đã được huỷ bỏ!",
                                "error"
                            );
                        },
                    });
                } else if (result.dismiss === Swal.DismissReason.cancel) {
                    swalWithBootstrapButtons.fire(
                        "Đã huỷ bỏ",
                        "Thao tác đã được huỷ bỏ",
                        "error"
                    );
                }
            });
    });

    $(".btnXemchitiet").click(function () {
        var id = $(this).data("id");
        $.ajax({
            url: "GetAllByCauHoiTuLuan",
            type: "POST",
            data: {
                id: id,
            },
            success: function (result) {
                $("#" + id)
                    .html(result)
                    .then(function () {
                        console.log(result);
                    });
            },
        });
    });
});
