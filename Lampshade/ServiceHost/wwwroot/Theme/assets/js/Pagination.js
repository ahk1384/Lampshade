const products = Array.from(document.querySelectorAll(".single-grid-product"));
const blogs = Array.from(document.querySelectorAll(".single-blog-post"));
const all = products.concat(blogs);
const itemsPerPage = 9;
const totalItems = all.length;
const totalPages = Math.ceil(totalItems / itemsPerPage);

let currentPage = 1;

function showPage(page) {
    currentPage = page;

    const start = (page - 1) * itemsPerPage;
    const end = start + itemsPerPage;

    // hide all products
    all.forEach(p => p.style.display = "none");

    // show current page products
    all.slice(start, end).forEach(p => {
        p.style.display = "block";
    });

    updateResultCount(start, end);
    renderPagination();
}

function updateResultCount(start, end) {
    const showingStart = start + 1;
    const showingEnd = Math.min(end, totalItems);

    document.getElementById("result-count").innerText =
        `Showing ${showingStart}-${showingEnd} of ${totalItems} (${totalPages} Pages)`;
}

function renderPagination() {
    const pagination = document.getElementById("pagination");
    pagination.innerHTML = "";

    for (let i = 1; i <= totalPages; i++) {
        const btn = document.createElement("li");
        btn.innerText = i;

        if (i === currentPage) {
            btn.classList.add("active");
        }

        btn.onclick = () => showPage(i);

        pagination.appendChild(btn);
    }
}

showPage(1);
