var toString = Object.prototype.toString;

!(function (window, jQuery) {
    "use strict";
    //Main Class
    var myapp = function () {
    };

    //Public Methods 1
    myapp.prototype.ajax = function (custInfo) {
        debugger;
        $.ajax({
            async: true,
            beforeSend: function (xhr) {
                if ('setHeader' in window && typeof setHeader == 'function')
                    setHeader(xhr);
            },
            cache: false,
            contentType: "application/json",
            crossDomain: true,
            data: JSON.stringify(custInfo),
            dataType: "json",
            processData: false,
            type: "POST",
            url: custInfo.url,
            success: function (data) {
                custInfo.callback.success(data, custInfo);
            },
            error: function (data) {
                custInfo.callback.error(data, custInfo);
            }
        });
    };

    //Public Methods 2
    myapp.prototype.notification = function (pObject) {
        pObject = pObject || {};
        pObject = {
            message: pObject.message || "",
            direction: pObject.direction || "right"
            , cssClass: pObject.cssClass || "default"
            , position: 'fixed'
            , top: "15px"
            , left: "50%"
            , display: "block"
            , marginLeft: "-180px"
            , notificationId: '#fignotificationbox'
            , width: "360px"
            , autoHide: 'autoHide' in pObject ? pObject.autoHide == false ? false : true : true
        };

        pObject.html = '<li class="fignotification-' + pObject.cssClass + '">' + pObject.message + '<span class="' + (pObject.autoHide == false ? " static" : "") + '"></span></li>';
        if ($(pObject.notificationId).length == 0)
            $('<div id="fignotificationbox" class="fignotification"><ul></ul></div>').css({ position: pObject.position, maxWidth: pObject.width, minWidth: 300, top: pObject.top, left: pObject.left, display: pObject.display, marginLeft: pObject.marginLeft }).appendTo('body');
        if (pObject.autoHide == false) {
            $(pObject.html).hide().prependTo('#fignotificationbox ul').slideDown();
            $('#fignotificationbox ul li span').on('click', function () { $(this).closest('li').remove(); });
        } else {
            $(pObject.html).hide().prependTo('#fignotificationbox ul').slideDown().delay(3000).slideUp(function () { $(this).remove(); }).on('click', function () { $(this).remove(); });
        }
    };

    //Public Methods 4
    myapp.prototype.alert = function (e) {
        PopupBoxHTML(e);
        var iconClassOk,
            buttonTextOk;

        if ('button' in e && 'save' in e.button) {
            iconClassOk = e.button.save.iconClass;
            buttonTextOk = e.button.save.text;
        } else {
            iconClassOk = "fa fa-check";
            buttonTextOk = "OK";
        }


        $('.ss-overlayer-container-footer :button.btn-danger').addClass('hide');
        $('.ss-overlayer-container-footer :button.btn-primary').html('<i class="' + (iconClassOk) + '"></i>' + buttonTextOk);
        $('.ss-overlayer-container-footer :button.btn-primary').click(function () {
            $(".ss-overlayer.modal.in").hide()
            $(".modal-backdrop.in").hide()
        });
    };

    //Public Methods 5
    myapp.prototype.confirm = function (e) {
        PopupBoxHTML(e);
    };

    myapp.prototype.modal = function (e) {
        window.myapp.confirm(e);
    };


    //Public Methods 6
    myapp.prototype.getById = function (ctrlId) {
        return $(ctrlId[0] == '#' ? ctrlId : '#' + ctrlId);
    }

    //Public Methods 7
    myapp.prototype.getByClass = function (ctrlClass) {
        return $(ctrlId[0] == '.' ? ctrlId : '.' + ctrlId);
    }



    //Private Method 1
    var PopupBoxHTML = function (e) { debugger
        var iconClassSave,
            buttonTextSave;

        if ('button' in e && 'save' in e.button) {
            iconClassSave = e.button.save.cssClass;
            buttonTextSave = e.button.save.text;
        } else {
            iconClassSave = "fa fa-check";
            buttonTextSave = "Yes";
        }

        var iconClassCancel,
            buttonTextCancel;

        if ('button' in e && 'cancel' in e.button) {
            iconClassCancel = e.button.cancel.cssClass;
            buttonTextCancel = e.button.cancel.text;
        } else {
            iconClassCancel = "fa fa-times";
            buttonTextCancel = "No";
        }


        var styleWidth,
            styleMaxHeight,
            styleOverflowY,
            styleContainer,
            styleContainerBody;

        if ('style' in e) {
            styleWidth = e.style.width;
            styleOverflowY = e.style.overflowY;
            styleMaxHeight = e.style.maxHeight;

            styleContainer = 'style = "max-width: ' + (styleWidth) + ';"';
            styleContainerBody = 'style = "overflow-y: ' + (styleOverflowY) + '; max-height: ' + (styleMaxHeight) + ';"';
        } else {
            styleContainer = '';
            styleContainerBody = '';
        }
        debugger;

        $('body .ss-overlayer').remove();
        var h = '<div class="ss-overlayer modal" data-backdrop="static" data-keyboard="false" role="dialog" style="overflow: auto;" tabindex="-1" rel="true">\
                    <div class="ss-overlayer-container" ' + (styleContainer) + '>\
                        <div class="ss-overlayer-container-header">' + e.title + '</div>\
                        <div class="ss-overlayer-container-body" '+ (styleContainerBody) + '>' + e.htmlBody + '</div>\
                        <div class="ss-overlayer-container-footer">\
                            <button type="button" class="btn btn-sm btn-primary ' + (iconClassSave) + '"><i class="' + (iconClassSave) + 'hide"></i> ' + (buttonTextSave) + '</button>\
                            <button type="button" class="btn btn-sm btn-danger ' + (iconClassCancel) + '"><i class="' + (iconClassCancel) + 'hide"></i> ' + (buttonTextCancel) + '</button>\
                        </div>\
                    </div>\
                </div>';
        $(h).appendTo('body');
        $('.ss-overlayer').modal('show').modalTop();



        console.log(e);

        $('.ss-overlayer-container-footer :button.btn-primary').on('click', function () {
            if (('valid' in e && e.valid == true) || e.isvalid === true) {
                window.myapp.modalClose();
            }
            //$('.ss-overlayer').modal('hide');
            //$('body .ss-overlayer').remove();
            if ('callback' in e && typeof e.callback === "function")
                e.callback(true);
        });
        $('.ss-overlayer-container-footer :button.btn-danger').on('click', function () {
            $('.ss-overlayer').modal('hide');
            $('body .ss-overlayer').remove();
            if ('callback' in e && typeof e.callback === "function")
                e.callback(false);
        });
    };

    myapp.prototype.modalClose = function () {
        $('.ss-overlayer').modal('hide');
        $('body .ss-overlayer').remove();
    }

    window.myapp = new myapp();


    //JQuery Methods
    if ('jQuery' in window) {
        //Jquery Method 1
        $.fn.modalTop = function () {
            return this.each(function () {
                if ($(".modal-backdrop").length > 1) {
                    var eq = $(".modal-backdrop").length - 2,
                        zIndex = $('.modal-backdrop:eq(' + eq + ')').css('zIndex');
                    $('.modal-backdrop:last').css('zIndex', parseInt(zIndex) + 15);
                    $(this).css('zIndex', parseInt(zIndex) + 20);
                }
            });
        };

        //Jquery Method 2
        $.fn.SAVYUIGRIDPAGING = function (PAGERDATA) {
            return this.each(function () {
                PAGERDATA.TOTALPAGE = Math.ceil(PAGERDATA.TOTALRECORD / PAGERDATA.PAGESIZE);

                if ("PAGERCOUNT" in PAGERDATA) {
                    PAGERDATA.PAGERCOUNT = !!PAGERDATA.PAGERCOUNT ? (PAGERDATA.PAGERCOUNT % 2 == 0 ? PAGERDATA.PAGERCOUNT - 1 : PAGERDATA.PAGERCOUNT) : 5;
                } else {
                    PAGERDATA.PAGERCOUNT = 5;
                }

                if ("PAGERTEXT" in PAGERDATA) {
                    PAGERDATA.PAGERTEXT = PAGERDATA.PAGERTEXT;
                } else {
                    PAGERDATA.PAGERTEXT = "Records";
                }


                var $ul = '<ul class="figpaging" id="pager' + ($(this).attr('id')) + '">',
                    li = '<li><button type="button"{0}>{1}</button></li>';


                $ul = '<span class="figpagingtext">' + PAGERDATA.PAGERTEXT + ' ' + (PAGERDATA.TOTALRECORD == 0 ? '0' : fnFormatedNumber(((PAGERDATA.PAGEINDEX - 1) * PAGERDATA.PAGESIZE + 1))) + '-' + fnFormatedNumber((PAGERDATA.PAGEINDEX * PAGERDATA.PAGESIZE > PAGERDATA.TOTALRECORD ? PAGERDATA.TOTALRECORD : PAGERDATA.PAGEINDEX * PAGERDATA.PAGESIZE)) + ' of ' + fnFormatedNumber((PAGERDATA.TOTALRECORD)) + " | Page " + fnFormatedNumber(PAGERDATA.TOTALPAGE == 0 ? 0 : PAGERDATA.PAGEINDEX) + " of " + fnFormatedNumber(PAGERDATA.TOTALPAGE) + '</span>' + $ul;

                if (PAGERDATA.PAGEINDEX == 1) {
                    $ul += (li + li).replace(/\{0\}/g, ' class="disabled colored"').replace(/\{1\}/, "<b>«</b>").replace(/\{1\}/, "<b>‹</b>");
                } else {
                    $ul += (li + li).replace(/\{0\}/, ' class="colored" page="1"').replace(/\{0\}/, ' class="colored" page="' + (PAGERDATA.PAGEINDEX - 1) + '"').replace(/\{1\}/, "<b>«</b>").replace(/\{1\}/, "<b>‹</b>");
                }

                for (var i = (PAGERDATA.TOTALPAGE > PAGERDATA.PAGERCOUNT && PAGERDATA.PAGEINDEX > Math.ceil(PAGERDATA.PAGERCOUNT / 2) ? (PAGERDATA.TOTALPAGE - 1 == PAGERDATA.PAGEINDEX || PAGERDATA.TOTALPAGE == PAGERDATA.PAGEINDEX) ? PAGERDATA.TOTALPAGE - (PAGERDATA.PAGERCOUNT - 1) : ((PAGERDATA.PAGEINDEX + Math.floor(PAGERDATA.PAGERCOUNT / 2)) > PAGERDATA.TOTALPAGE ? (PAGERDATA.TOTALPAGE - PAGERDATA.PAGERCOUNT + 1) : (PAGERDATA.PAGEINDEX - Math.floor(PAGERDATA.PAGERCOUNT / 2))) : 1) ;
                           i <= (PAGERDATA.TOTALPAGE > PAGERDATA.PAGERCOUNT ? PAGERDATA.TOTALPAGE > PAGERDATA.PAGERCOUNT && PAGERDATA.PAGEINDEX > Math.ceil(PAGERDATA.PAGERCOUNT / 2) ? (PAGERDATA.PAGEINDEX + (Math.floor(PAGERDATA.PAGERCOUNT / 2))) > PAGERDATA.TOTALPAGE ? PAGERDATA.TOTALPAGE : (PAGERDATA.PAGEINDEX + (Math.floor(PAGERDATA.PAGERCOUNT / 2))) : PAGERDATA.PAGERCOUNT : PAGERDATA.TOTALPAGE) ;
                           i++) {
                    $ul += li.replace(/\{0\}/, ' page="' + (i) + '"' + (i == PAGERDATA.PAGEINDEX ? ' class="active"' : "")).replace(/\{1\}/, i);
                }

                if (PAGERDATA.TOTALPAGE == 0) {
                    $ul += li.replace(/\{0\}/, ' class="active"').replace(/\{1\}/, 0);
                    $ul += (li + li).replace(/\{0\}/g, ' class="colored disabled"').replace(/\{1\}/, "›").replace(/\{1\}/, "<b>»</b>");
                }
                else if (PAGERDATA.PAGEINDEX == PAGERDATA.TOTALPAGE) {
                    $ul += (li + li).replace(/\{0\}/g, ' class="colored disabled"').replace(/\{1\}/, "›").replace(/\{1\}/, "<b>»</b>");
                } else {
                    $ul += (li + li).replace(/\{0\}/, ' class="colored" page="' + (PAGERDATA.PAGEINDEX + 1) + '"').replace(/\{0\}/, ' class="colored" page="' + PAGERDATA.TOTALPAGE + '"').replace(/\{1\}/, "<b>›</b>").replace(/\{1\}/, "<b>»</b>");
                }



                /**
                * DROPDOWN START
                **/

                if ('PAGING' in PAGERDATA && PAGERDATA.PAGING == true && PAGERDATA.TOTALRECORD > 0) {
                    $ul += '<li><select title="Records per page">';
                    if ('RECORDPERPAGE' in PAGERDATA == false || toString.call(PAGERDATA.RECORDPERPAGE) != '[object Array]')
                        PAGERDATA.RECORDPERPAGE = [5, 10, 15, 20, 25];

                    for (var i = 0; i < PAGERDATA.RECORDPERPAGE.length; i++) {
                        $ul += '<option ';
                        if (PAGERDATA.PAGESIZE == PAGERDATA.RECORDPERPAGE[i])
                            $ul += 'selected="selected" ';
                        $ul += 'value="' + PAGERDATA.RECORDPERPAGE[i] + '">' + PAGERDATA.RECORDPERPAGE[i] + '</option>';
                    }
                    //$ul += '<option value="' + PAGERDATA.TOTALRECORD + '" ' + (PAGERDATA.TOTALRECORD == PAGERDATA.PAGESIZE ? "selected='selected'" : "") + '>All</option>';
                    $ul += '</select></li>';
                }
                /**
                * DROPDOWN END
                **/

                $ul += '</ul>';

                $(this).html($ul);



                $('#pager' + $(this).attr('id')).find('button').on('click', function () {
                    PAGERDATA.PAGEINDEX = parseInt($(this).attr('page'));
                    PAGERDATA.CALLBACK(PAGERDATA);
                });

                $('#' + $(this).attr('id') + ' select').on('change', function (E) {
                    PAGERDATA.PAGEINDEX = 1;
                    PAGERDATA.PAGESIZE = parseInt($(this).find('option:selected').val());
                    PAGERDATA.CALLBACK(PAGERDATA);
                });

                /**
                * SORTING START
                **/
                if ('SORTING' in PAGERDATA && PAGERDATA.SORTING == true) { // Sorting records
                    var $thead = $(this).closest('.figtable-container').find('.figtable-header th.sort');


                    $thead.each(function () {
                        if ($(this).find('span').length == 0) {
                            if (PAGERDATA.SORTORDER == null || PAGERDATA.SORTORDER == undefined) {
                                $(this).append('<span class="fa fa-sort pull-right"></span>');
                            } else if (PAGERDATA.SORTORDER.toUpperCase().indexOf('ASC') != -1 && PAGERDATA.SORTCOLUMN == $(this).attr('data-db-field')) {
                                $(this).append('<span class="fa fa-sort-asc pull-right"></span>');
                            } else if (PAGERDATA.SORTORDER.toUpperCase().indexOf('DESC') != -1 && PAGERDATA.SORTCOLUMN == $(this).attr('data-db-field')) {
                                $(this).append('<span class="fa fa-sort-desc pull-right"></span>');
                            } else {
                                $(this).append('<span class="fa fa-sort pull-right"></span>');
                            }
                        }
                    });


                    $thead.off('click').on('click', function () {

                        $thead.not($(this)).removeClass('asc desc').parent().find('span').remove();
                        $(this).find('span').remove();

                        PAGERDATA.SORTCOLUMN = $(this).attr('data-db-field');

                        if ($(this).hasClass('asc')) {
                            $(this).toggleClass('desc asc').append('<span class="fa fa-sort-desc pull-right"></span>');
                            PAGERDATA.SORTORDER = 'DESC';
                        }
                        else if ($(this).hasClass('desc')) {
                            $(this).removeClass('asc desc');
                            PAGERDATA.SORTORDER = PAGERDATA.SORTCOLUMN = '';
                            $(this).append('<span class="fa fa-sort pull-right"></span>');
                        } else {
                            $(this).addClass('asc').removeClass('desc').append('<span class="fa fa-sort-asc pull-right"></span>');
                            PAGERDATA.SORTORDER = 'ASC';
                        }
                        PAGERDATA.CALLBACK(PAGERDATA);
                    });
                } else {
                    $(this).closest('.figtable-container').find('.figtable-header th').removeClass('sort');
                }
                /**
                * SORTING END
                **/

                /**
                * FILTERING START
                **/

                if ('FILTERING' in PAGERDATA && PAGERDATA.FILTERING == true) {
                    console.log(1);
                    $(this).closest('.figtable-container').find('.figtable-header input:text')
                        .off('keyup').on('keyup', function (e) {

                            window.keytimer;
                            clearTimeout(window.keytimer);
                            window.keytimer = setTimeout(function () {
                                PAGERDATA.PAGEINDEX = 1;
                                return PAGERDATA.CALLBACK(PAGERDATA);
                            }, 300);
                        });

                    $(this).closest('.figtable-container').find('.figtable-header select,.figtable-header input:checkbox,.figtable-header input:radio')
                       .off('change').on('change', function (e) {
                           PAGERDATA.PAGEINDEX = 1;
                           return PAGERDATA.CALLBACK(PAGERDATA);
                       });
                }

                /**
                * FILTERING END
                **/

                function fnFormatedNumber(number) {
                    return number < 10 ? number : number;
                }


                if ('HEIGHT' in PAGERDATA && typeof PAGERDATA.HEIGHT == 'number') {
                    $(this).closest('.figtable-container').find('.figtable-body').css({ maxHeight: PAGERDATA.HEIGHT });
                }
                else {
                    $(this).closest('.figtable-container').find('.figtable-body').css({ maxHeight: 'auto' });
                }

                /**
                * HEADER HORIZONTAL SCROLLING START
                **/

                $(this).closest('.figtable-container').find('.figtable-body:first').off('scroll').on('scroll', function (e) {
                    e.target = e.target || e.srcElement; $(this).closest('.figtable-container').find('.figtable-header table').css({
                        marginLeft: -e.target.scrollLeft + "px"
                    });
                });
            });
        }

        //Jquery Method 3
        $.fn.figloader = function (e, message) {
            return this.each(function () {
                if (e == 'show') {
                    if ($('.figloaderlayer').length == 0) {
                        $(this).attr('data-loader-content', message).addClass('figloader');
                        $(this).append('<div class="figloaderlayer"></div>');
                    }
                }
                else if (e == 'hide') {
                    $(this).find('.figloaderlayer').remove();
                    $(this).removeClass('figloader');
                }
            });
        };

        //Jquery Method 4
        $.fn.resetRadioSelection = function () {
            return this.find("input[type='radio']").on('click', function () {

                var previousValue = $(this).attr('previousValue');
                var name = $(this).attr('name');

                if (previousValue == 'checked') {
                    $(this).removeAttr('checked');
                    $(this).attr('previousValue', 'unchecked');
                }
                else {
                    $("input[name=" + name + "]:radio").attr('previousValue', 'unchecked');
                    $(this).attr('previousValue', 'checked');
                }
            });
        };


        $.figBindSelectOption = function (array, textField, valueField, modalField, DefaultValue, selectedValue) {

            var returnValue = '';
            if (typeof DefaultValue == "string")
                returnValue += '<option value="" selected="selected">' + DefaultValue + '</option>';

            $.each(array, function (key, value) {
                returnValue += '<option' + (modalField != null ? ' data-modal="' + array[key][modalField] : "") + '"  value="' + array[key][valueField] + '"' + (selectedValue == array[key][valueField] ? " selected='selected'" : "") + '>' + array[key][textField] + '</option>';
            });

            return returnValue;
        };

        $.figBindRangeSelectOption = function (min, max, isDefault) {
            var returnValue = '';
            if (!!isDefault)
                returnValue += '<option value="" selected="selected"></option>';
            if (min <= max) {
                for (min; min <= max; min++) {
                    returnValue += '<option value="' + (min > 9 ? min : '0' + min) + '">' + (min > 9 ? min : '0' + min) + '</option>';
                }
            } else {
                for (max; max <= min; min--) {
                    returnValue += '<option value="' + (min > 9 ? min : '0' + min) + '">' + (min > 9 ? min : '0' + min) + '</option>';
                }
            }
            return returnValue;
        };
    }

}(window, jQuery));


/*
EX.
1). ajax
*/

var requestJson = {
    data: {
        PInfo: {

        }
    },
    url: "",
    callback: {
        success: function () { },
        error: function () { }
    }
};

//myapp.ajax(requestJson);

/*
EX.
2). notification
*/

var pObject = {
    message: "Records saved successfully",
    cssClass: "success"
}

//myapp.notification(pObject);

/*
EX.
3). loader
*/

//$('body').loader('show');

//myapp.alert("Confirmation", "Are you sure you want to delete this record?", null)

//myapp.confirm("Confirmation", "Are you sure you want to delete this record?", null)