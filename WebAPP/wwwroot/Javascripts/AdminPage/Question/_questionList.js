$(document).ready(function () {
    $(".btnSuaDiem").click(function () {
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
                    showScoreInputDialog(
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
    $(".btnXoaCauHoiTracNghiem").click(function () {
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
                confirmButtonText: "Xác nhận",
                cancelButtonText: "Huỷ bỏ",
                reverseButtons: true,
            })
            .then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: "DeleteTracNghiem",
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
                                    url: "GetAllByCategoryId",
                                    type: "POST",
                                    data: {
                                        id: intCategoryId,
                                    },
                                    success: function (result) {
                                        $("#listQuestion").html(result);
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
                }
            });
    });
    $(".btnSuaCauHoi").click(function () {
        var id = $(this).data("id");
        $.ajax({
            url: "GetCauHoiTracNghiemById",
            type: "POST",
            data: {
                id: id,
            },
            success: function (result) {
                $("#NoiDungCauHoiTracNghiem").html(result);
                // Open the modal
                $("#staticBackdrop").modal("show");
            },
        });
    });
});

async function showScoreInputDialog(id, intCategoryId, remainScore, thisScore) {
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
            url: "UpdateScore",
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
                        url: "GetAllByCategoryId",
                        type: "POST",
                        data: {
                            id: intCategoryId,
                        },
                        success: function (result) {
                            $("#listQuestion").html(result);
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
                            if (result.result != 10) {
                                $("#theThongBao").removeClass(
                                    "text-bg-success"
                                );
                                $("#theThongBao").addClass("text-bg-danger");
                            } else {
                                $("#theThongBao").removeClass("text-bg-danger");
                                $("#theThongBao").addClass("text-bg-success");
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
                            if (result.sl != 0) {
                                $("#theThongBaoChuaGan").removeClass("text-bg-success")
                                $("#theThongBaoChuaGan").addClass("text-bg-danger")
                            } else {
                                $("#theThongBaoChuaGan").removeClass("text-bg-danger")
                                $("#theThongBaoChuaGan").addClass("text-bg-success")
                            }
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
                Swal.fire("Lỗi máy chủ", "Thao tác đã được huỷ bỏ!", "error");
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
