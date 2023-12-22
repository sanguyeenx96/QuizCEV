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
                        $.ajax({
                            url: "GetTotalScore",
                            type: "POST",
                            data: {
                                categoryId: intCategoryId,
                            },
                            success: function (result) {
                                $("#countDiemso").html(result.result);
                                if (result.result !== 10) {
                                    $("#logoDiemdagan i").html('<i class="bi-x-circle-fill" style="color:red"></i>');
                                } else {
                                    $("#logoDiemdagan i").html('<i class="bi-check-circle-fill" style="color:green"></i>');
                                }
                            },
                        });
                        $.ajax({
                            url: "GetCauHoiNullScore",
                            type: "POST",
                            data: {
                                id: intCategoryId,
                            },
                            success: function (result) {
                                $("#countChuaGan").html(result.sl);
                                if (result.sl !== 0) {
                                    $("#logoChuagandiem i").html('<i class="bi-x-circle-fill" style="color:red"></i>');

                                } else {
                                    $("#logoChuagandiem i").html('<i class="bi-check-circle-fill" style="color:green"></i>');

                                }
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

    $(".btnThemNoiDungThaoTac").click(async function () {
        var categoryId = $(this).data("abc");
        var intCategoryId = parseInt(categoryId);
        var id = $(this).data("id");
        var intId = parseInt(id);
        const { value: text } = await Swal.fire({
            input: "textarea",
            inputLabel: "Nội dung thao tác mới :",
            inputPlaceholder: "Nhập nội dung thao tác...",
            showCancelButton: true,
            confirmButtonText: "Xác nhận",
            cancelButtonText: "Huỷ bỏ",
            reverseButtons: true,
        });
        if (text) {
            $.ajax({
                url: "CreateNoiDungTrinhTuThaoTac",
                type: "POST",
                data: {
                    CauHoiTuLuanId: intId,
                    Text: text,
                },
                success: function (result) {
                    Swal.fire({
                        icon: "success",
                        title: "Thêm nội dung thành công",
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
                },
            });
        }
    });

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

    $(".btnSuaCauHoiTuLuan").click(async function () {
        var id = $(this).attr("data-id");
        var noidungcauhoi = $(this).data("tencauhoi");
        var categoryId = $(this).data("abc");
        var intId = parseInt(id);
        var intCategoryId = parseInt(categoryId);

        const { value: text } = await Swal.fire({
            input: "textarea",
            inputLabel: "Sửa tiêu đề câu hỏi :",
            inputValue: noidungcauhoi,
            inputAttributes: {
                "aria-label": "Type your message here",
            },
            showCancelButton: true,
            confirmButtonText: "Xác nhận",
            cancelButtonText: "Huỷ bỏ",
            reverseButtons: true,
        });
        if (text) {
            $.ajax({
                url: "UpdateTextCauHoiTuLuan",
                type: "POST",
                data: {
                    id: intId,
                    Text: text,
                },
                success: function (result) {
                    Swal.fire({
                        icon: "success",
                        title: "Sửa tiêu đề câu hỏi thành công",
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
                },
            });
        }
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
                title: "Xoá toàn bộ câu hỏi tự luận?",
                text: "Toàn bộ dữ liệu liên quan tới câu hỏi sẽ bị xoá",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Xác nhận",
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
        var catId = $(this).attr("data-catId");
        catId = parseInt(catId);
        $.ajax({
            url: "GetAllByCauHoiTuLuan",
            type: "POST",
            data: {
                id: id,
                catId:catId
            },
            success: function (result) {
                $("#" + id).html(result);
            },
        });
    });
});
