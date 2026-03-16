const cookieName = "cart-items";

function addToCart(productId, name, unitPrice, picture, isInStock, discountRate) {
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
    } 
    else {
        const product = {
            productId,
            name,
            unitPrice,
            picture,
            count,
            totalItemPrice,
            isInStock,
            discountRate,
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
    const cartItemsWrapper = $("#cart_items_wrapper");
    cartItemsWrapper.html('');
    products.forEach(x => {
        const total = (x.unitPrice-x.discountAmount) * x.count
        const product = `
            <div class="single-cart-item">
                <div class="image">
                    <a href="single-product.html">
                        <img src="/ProductPictures/${x.picture}" class="img-fluid" alt="">
                    </a>
                </div>
                <div class="content">
                    <p class="product-title">
                        <a href="single-product.html">محصول: ${x.name}</a>
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

