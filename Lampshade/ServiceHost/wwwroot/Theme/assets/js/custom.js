const cookieName = "cart-items";
function addToCart(productId, name, unitPrice, picture, isInStock, discountRate, productSlug) {
    let products = $.cookie(cookieName);
    if (products === undefined) {
        products = [];
    } else {
        products = JSON.parse(products);
    }
    const count = $("#productCount").val();
    const totalItemPrice = unitPrice * count;
    const discountAmount = count * unitPrice * discountRate / 100;
    const itemPayAmount = totalItemPrice - discountAmount;
    const currentProduct = products.find(x => x.productId === productId);
    if (currentProduct !== undefined) {
        products.find(x => x.productId === productId).count = parseInt(currentProduct.count) + parseInt(count);
    } else {
        const product = {
            productId,
            name,
            unitPrice,
            picture,
            count,
            totalItemPrice,
            isInStock,
            discountRate,
            productSlug,
            discountAmount,
            itemPayAmount
        }
        products.push(product);
    }
    $.cookie(cookieName, JSON.stringify(products), {expires: 2, path: "/"});
    updateCart();

}

function updateCart() {
    let products = $.cookie(cookieName);
    products = JSON.parse(products);
    $("#cart_items_count").text(products.length);
    $("#mobile-menu-counter").text(products.length);
    const cartItemsWrapper = $("#cart_items_wrapper");
    cartItemsWrapper.html('');
    products.forEach(x => {
        const total = (x.unitPrice - x.discountAmount) * x.count
        const product = `
            <div class="single-cart-item">
                <div class="image">
                    <a href="/Product/${x.productSlug}">
                        <img src="/ProductPictures/${x.picture}" class="img-fluid" alt="">
                    </a>
                </div>
                <div class="content">
                    <p class="product-title">
                        <a href="/Product/${x.productSlug}">محصول: ${x.name}</a>
                    </p>
                    <p class="count">تعداد: ${x.count}</p>
                    <p class="count">قیمت : ${x.unitPrice - x.discountAmount}</p>
                    <p class="count">هزینه کل: ${total}</p>
                </div>
            </div>`;

        cartItemsWrapper.append(product);

    });
}

function removeFromCart(id) {
    let products = $.cookie(cookieName);
    products = JSON.parse(products);
    const itemToRemove = products.findIndex(x => x.productId === id);
    products.splice(itemToRemove, 1);
    $.cookie(cookieName, JSON.stringify(products), {expires: 2, path: "/"});
    updateCart();
}

function changeCartItemCount(id, quantity) {

    fetch(`/Cart?handler=ChangeItemCount&productId=${id}&count=${quantity}`)
        .then(response => response.json())
        .then(data => {
            console.log("Cart updated");
        })
        .catch(error => console.error(error));
    var products = $.cookie(cookieName);
    products = JSON.parse(products);
    const productIndex = products.findIndex(x => x.productId == id);
    products[productIndex].count = quantity;
    const product = products[productIndex];
    const newPrice = parseInt(product.unitPrice) * parseInt(quantity);
    $.cookie(cookieName, JSON.stringify(products), {expires: 2, path: "/"});
    updateCart();

    const settings = {
        "url": "https://api.amirhkz.ir/api/Inventory",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": JSON.stringify({"productId": id, "count": quantity})
    };

    $.ajax(settings).done(function (data) {
        if (data.isStock == false) {
            const warningsDiv = $('#productStockWarnings');
            if ($(`#${id}`).length == 0) {
                warningsDiv.append(`
                    <div class="alert alert-warning" id="${id}">
                        <i class="fa fa-warning"></i> کالای
                        <strong>${data.productName}</strong>
                        در انبار کمتر از تعداد درخواستی موجود است.
                    </div>
                `);
            }
        } else {
            if ($(`#${id}`).length > 0) {
                $(`#${id}`).remove();
            }
        }
    });
}

