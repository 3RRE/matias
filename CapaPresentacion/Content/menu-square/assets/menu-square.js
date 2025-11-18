// Menu Square JS
document.addEventListener('keydown', (event) => {
    if (event.ctrlKey && event.key == 'm' || event.ctrlKey && event.key == 'M') {
        toggleMenuSquare()
        defaultMenuSquare()
        viewMenuCurrent()
    }

    if (event.key === "Escape") {
        $('html').removeClass('ms-overflow-hidden')
        $('body').removeClass('menu-full-open')
        defaultMenuSquare()
    }
}, true);

$(document).on('click', '.menu-square-btn', function () {
    toggleMenuSquare()
    viewMenuCurrent()
})

$(document).on('click', '.menu--close', function () {
    toggleMenuSquare()
    defaultMenuSquare()
})

$(document).on('click', '.button--module', function (event) {
    if ($(this).attr('href') == '#') {
        event.preventDefault()

        var module = $(this)
        var level = 2
        var parent = module.parent().attr('data-parent')
        var key = module.parent().attr('data-key')

        if (TokenProgresivo.isModule(key)) {
            TokenProgresivo.isValid()
        }

        $('.menu-square--wrap').addClass('ms-inactive')
        $(`.submenu--square[data-parent="${key}"]`).addClass('ms-active')
        $('.header-ms-menu-back').addClass('ms-active')
        $('.header-ms-menu-back').attr('data-menu', level)
        $('.menu-ms-bg-level').attr('transition-style', 'in:custom:circle-swoop:lvl-2')
        $('.waves---wrapper').addClass('ms-active')
        $('.waves---wrapper').html(wavesWrap())
        noDataMenuSquareModule(key)
        topScrollMS()
        setMenuCurrentLvl(level, parent, key)
    } else {
        toggleMenuSquare()
    }
})

$(document).on('click', '.button--submodule', function (event) {
    if ($(this).attr('href') == '#') {
        event.preventDefault()
        var submodule = $(this)
        var level = 3
        var parent = submodule.parent().attr('data-parent')
        var key = submodule.parent().attr('data-key')
        $('.submenu-square--wrap').addClass('ms-inactive')
        $(`.subsubmenu--square[data-parent="${key}"]`).addClass('ms-active')
        $('.header-ms-menu-back').attr('data-menu', level)
        $('.menu-ms-bg-level').attr('transition-style', 'in:wipe:bottom-left:lvl-3')
        $('.waves---wrapper').removeClass('ms-active')
        $('.waves---wrapper').html('')
        $('.ms-circles---wrapper').addClass('ms-active')
        $('.ms-circles---wrapper').html(circlesWrap())
        noDataMenuSquareSubModule(key)
        topScrollMS()
        setMenuCurrentLvl(level, parent, key)
    } else {
        toggleMenuSquare()
    }
})

$(document).on('click', '.header-ms-menu-back', function (event) {
    event.preventDefault()
    if ($(this).attr('data-menu') == 2) {
        $('.menu-square--wrap').removeClass('ms-inactive')
        $('.submenu--square').removeClass('ms-active')
        $(this).removeClass('ms-active')
        $('.header-ms-menu-back').attr('data-menu', 1)
        $('.menu-ms-bg-level').attr('transition-style', 'out:custom:circle-swoop')
        $('.waves---wrapper').removeClass('ms-active')
        $('.waves---wrapper').html('')
        updateMenuCurrentLvl(1)
    }

    if ($(this).attr('data-menu') == 3) {
        $('.submenu-square--wrap').removeClass('ms-inactive')
        $('.subsubmenu--square').removeClass('ms-active')
        $('.header-ms-menu-back').attr('data-menu', 2)
        $('.menu-ms-bg-level').attr('transition-style', 'in:custom:circle-swoop:lvl-2')
        $('.ms-circles---wrapper').removeClass('ms-active')
        $('.ms-circles---wrapper').html('')
        $('.waves---wrapper').addClass('ms-active')
        $('.waves---wrapper').html(wavesWrap())
        updateMenuCurrentLvl(2)
    }

    $('.nodata-ms--wrap').removeClass('ms-active')
    topScrollMS()
})

$(document).on('click', '.ms-breadcrumb--back', function (event) {
    event.preventDefault()
    defaultMenuSquare()
    topScrollMS()
    updateMenuCurrentLvl(1)
})

$(document).on('click', '.ms-breadcrumb--subback', function (event) {
    event.preventDefault()
    $('.submenu-square--wrap').removeClass('ms-inactive')
    $('.subsubmenu--square').removeClass('ms-active')
    $('.header-ms-menu-back').attr('data-menu', 2)
    $('.menu-ms-bg-level').attr('transition-style', 'in:custom:circle-swoop:lvl-2')
    $('.ms-circles---wrapper').removeClass('ms-active')
    $('.ms-circles---wrapper').html('')
    $('.waves---wrapper').addClass('ms-active')
    $('.waves---wrapper').html(wavesWrap())

    $('.nodata-ms--wrap').removeClass('ms-active')
    topScrollMS()
    updateMenuCurrentLvl(2)
})

function toggleMenuSquare() {
    $('html').toggleClass('ms-overflow-hidden')
    $('body').toggleClass('menu-full-open')
}

function defaultMenuSquare() {
    $('.menu-square--wrap').removeClass('ms-inactive')
    $('.submenu-square--wrap').removeClass('ms-inactive')
    $('.submenu--square').removeClass('ms-active')
    $('.subsubmenu--square').removeClass('ms-active')
    $('.header-ms-menu-back').removeClass('ms-active')
    $('.header-ms-menu-back').attr('data-menu', 1)
    $('.nodata-ms--wrap').removeClass('ms-active')
    $('.menu-ms-bg-level').attr('transition-style', 'out:custom:circle-swoop')
    $('.waves---wrapper').removeClass('ms-active')
    $('.waves---wrapper').html('')
    $('.ms-circles---wrapper').removeClass('ms-active')
    $('.ms-circles---wrapper').html('')
    topScrollMS()
}

function noDataMenuSquareModule(parent) {
    var items = $(`.submenu--square[data-parent="${parent}"] .submenu--item[data-parent="${parent}"]`)
    if (!items.length) $('.nodata-ms--wrap').addClass('ms-active')
}

function noDataMenuSquareSubModule(parent) {
    var items = $(`.subsubmenu--square[data-parent="${parent}"] .subsubmenu--item[data-parent="${parent}"]`)
    if (!items.length) $('.nodata-ms--wrap').addClass('ms-active')
}

function wavesWrap() {
    var wrap = `
    <svg class="waves" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 24 150 28" preserveAspectRatio="none" shape-rendering="auto">
        <defs>
            <path id="gentle-wave" d="M-160 44c30 0 58-18 88-18s 58 18 88 18 58-18 88-18 58 18 88 18 v44h-352z" />
        </defs>
        <g class="waves-parallax">
            <use xlink:href="#gentle-wave" x="48" y="0" fill="rgba(255,255,255,0.04)" />
            <use xlink:href="#gentle-wave" x="48" y="3" fill="rgba(255,255,255,0.03)" />
            <use xlink:href="#gentle-wave" x="48" y="5" fill="rgba(255,255,255,0.02)" />
            <use xlink:href="#gentle-wave" x="48" y="7" fill="rgba(255,255,255,0.01)" />
        </g>
    </svg>
    `

    return wrap
}

function circlesWrap() {
    var wrap = `
    <ul class="ms-circles">
        <li></li>
        <li></li>
        <li></li>
        <li></li>
        <li></li>
        <li></li>
        <li></li>
        <li></li>
        <li></li>
        <li></li>
    </ul>
    `

    return wrap
}

function topScrollMS() {
    document.getElementsByClassName('mf-content-wrap')[0].scrollTop = 0
}

// menu current
function setMenuCurrentLvl(level, parent, key) {
    var data = {
        level: level,
        parent: parent,
        key: key
    }

    localStorage.setItem('MSCurrentLevel', JSON.stringify(data))
}

function getMenuCurrentLvl() {
    var keyMenu = JSON.parse(localStorage.getItem('MSCurrentLevel'))

    return keyMenu
}

function updateMenuCurrentLvl(level) {
    var data = getMenuCurrentLvl()

    if (level == 1) {
        localStorage.removeItem('MSCurrentLevel')
    }

    if (level == 2) {
        setMenuCurrentLvl(level, 'parent', data.parent)
    }
}

function viewMenuCurrent() {
    var data = getMenuCurrentLvl()

    if (data) {

        var level = data.level
        var parent = data.parent
        var key = data.key

        $('.menu-square--wrap').addClass('ms-inactive')

        if (level == 2) {
            $(`.submenu--square[data-parent="${key}"]`).addClass('ms-active')
            $('.header-ms-menu-back').addClass('ms-active')
            $('.header-ms-menu-back').attr('data-menu', level)
            $('.menu-ms-bg-level').attr('transition-style', 'in:custom:circle-swoop:lvl-2')
            $('.waves---wrapper').addClass('ms-active')
            $('.waves---wrapper').html(wavesWrap())
            noDataMenuSquareModule(key)
        }

        if (level == 3) {
            $(`.submenu--square[data-parent="${parent}"]`).addClass('ms-active')
            $('.submenu-square--wrap').addClass('ms-inactive')
            $(`.subsubmenu--square[data-parent="${key}"]`).addClass('ms-active')
            $('.header-ms-menu-back').addClass('ms-active')
            $('.header-ms-menu-back').attr('data-menu', level)
            $('.menu-ms-bg-level').attr('transition-style', 'in:wipe:bottom-left:lvl-3')
            $('.ms-circles---wrapper').addClass('ms-active')
            $('.ms-circles---wrapper').html(circlesWrap())
            noDataMenuSquareSubModule(key)
        }

        topScrollMS()

        if (TokenProgresivo.isModule(key)) {
            TokenProgresivo.isValid()
        }
    }
}
// menu current

// render menu
function renderMenu(data) {

    var menuSquareWrap = $('.menu-square--wrap')
    var subMenuSquareWrap = $('.submenu-square--wrap')
    var subSubMenuSquareWrap = $('.subsubmenu-square--wrap')

    data.map(function (menu) {
        if (menu.status) {

            menuSquareWrap.find('.ms--items').append(itemMenu('parent', menu, 1))

            if (menu.submenu) {

                subMenuSquareWrap.append(subMenuWrap(menu))

                menu.submenu.map(function (submenu) {
                    if (submenu.status) {

                        subMenuSquareWrap.find(`.submenu--square[data-parent="${menu.key}"] .ms--subitems`).append(itemMenu(menu.key, submenu, 2))

                        if (submenu.submenu) {

                            subSubMenuSquareWrap.append(subSubMenuWrap(submenu, menu))

                            submenu.submenu.map(function (subsubmenu) {
                                if (subsubmenu.status) {

                                    subSubMenuSquareWrap.find(`.subsubmenu--square[data-parent="${submenu.key}"] .ms--subsubitems`).append(itemMenu(submenu.key, subsubmenu, 3))

                                }
                            })
                        }
                    }
                })
            }
        }
    })
}

function subMenuWrap(parent) {
    var subMenu = `
        <!-- submenu -->
        <div class="submenu-square submenu--square" data-parent="${parent.key}">
            <!-- header submenu -->
            <div class="submenu-square-header">
                <h3 class="sms-header-title sms--title">${parent.name}</h3>
                <ul class="ms-breadcrumb">
                    <li class="ms-breadcrumb-item"><a href="#" class="ms-breadcrumb--back">Módulos</a></li>
                    <li class="ms-breadcrumb-item"><i class="las la-angle-right"></i></li>
                    <li class="ms-breadcrumb-item active"><span>Módulo ${parent.name}</span></li>
                </ul>
            </div>
            <!-- header submenu -->
            <!-- row -->
            <div class="row gp-4 ms--subitems"></div>
            <!-- row -->
        </div>
        <!-- submenu -->
        `

    return subMenu
}

function subSubMenuWrap(parent, superParent) {
    var subSubMenu = `
        <!-- subsubmenu -->
        <div class="subsubmenu-square subsubmenu--square" data-parent="${parent.key}">
            <!-- header subsubmenu -->
            <div class="subsubmenu-square-header">
                <h3 class="ssms-header-title ssms--title">${parent.name}</h3>
                <ul class="ms-breadcrumb">
                    <li class="ms-breadcrumb-item"><a href="#" class="ms-breadcrumb--back">Módulos</a></li>
                    <li class="ms-breadcrumb-item"><i class="las la-angle-right"></i></li>
                    <li class="ms-breadcrumb-item"><a href="#" class="ms-breadcrumb--subback">Módulo ${superParent.name}</a></li>
                    <li class="ms-breadcrumb-item"><i class="las la-angle-right"></i></li>
                    <li class="ms-breadcrumb-item active"><span>${parent.name}</span></li>
                </ul>
            </div>
            <!-- header subsubmenu -->
            <!-- row -->
            <div class="row gp-4 ms--subsubitems"></div>
            <!-- row -->
        </div>
        <!-- subsubmenu -->
        `

    return subSubMenu
}

function itemMenu(key, menu, level) {

    var pathBase = $("#BasePath").val()
    var itemClass = 'menu--item'
    var buttonClass = 'button--module'
    var iconWrap = `<img width="32px" src="${pathBase+menu.iconLink}" />`
    var hasSubMenuWrap = ''

    if (level == 2) {
        itemClass = 'submenu--item'
        buttonClass = 'button--submodule'
        iconWrap = `<i class="${menu.iconClass}"></i>`
    }

    if (level == 3) {
        itemClass = 'subsubmenu--item'
        buttonClass = 'button--subsubmodule'
        iconWrap = `<i class="${menu.iconClass}"></i>`
    }

    if (menu.submenu) {
        hasSubMenuWrap = `<div class="msi-has-submenu"><i class="las la-angle-right"></i></div>`
    }

    var linkMenu = menu.to == '#' ? menu.to : pathBase + menu.to

    var activeMenu = linkMenu == location.href ? 'active' : ''

    var item = `
        <!-- col -->
        <div class="col-xs-6 col-sm-4 col-md-3 col-lg-2 col-gp ${itemClass}" data-parent="${key}" data-key="${menu.key}" data-name="${menu.name}">
            <!-- item -->
            <a href="${linkMenu}" class="menu-square-item ${activeMenu} ${buttonClass}">
                <div class="msi-content">
                    <div class="msi-icon msi-icon-hover">
                        ${iconWrap}
                    </div>
                    <div class="msi-title msi-title-clamp">${menu.name}</div>
                </div>
                ${hasSubMenuWrap}
            </a>
            <!-- item -->
        </div>
        <!-- col -->
        `

    return item
}

function getMenu(keys) {
    var pathBase = $("#BasePath").val()
    var fileHash = ((+new Date) + Math.random() * 100).toString(32)
    $.ajax({
        url: `${pathBase}/Content/menu-square/menu.json?v=${fileHash}`,
        type: "GET",
        contentType: "application/json",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            response.map(function (menu) {

                if (keys.includes(menu.key)) {

                    menu.status = 1

                    if (menu.submenu) {

                        menu.submenu.map(function (submenu) {

                            if (keys.includes(submenu.key)) {
       
                                submenu.status = 1

                                if (submenu.submenu) {

                                    submenu.submenu.map(function (subsubmenu) {

                                        if (keys.includes(subsubmenu.key)) {

                                            subsubmenu.status = 1

                                        }
                                    })
                                }
                            }
                        })
                    }
                }
            })

            renderMenu(response)
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}

function getMenuUsuario() {
    var pathBase = $("#BasePath").val()
    var keys = []
    $.ajax({
        url: `${pathBase}/Usuario/ListadoMenus`,
        type: "POST",
        contentType: "application/json",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {

            var listado = response.dataResultado;

            if (listado.length > 0) {
                $.each(listado, function (index, value) {
                    var key = value.WEB_PMeDataMenu;
                    keys.push(key)
                });
            }
        },
        complete: function () {
            getMenu(keys)
            $.LoadingOverlay("hide");
        }
    });
}

getMenuUsuario()