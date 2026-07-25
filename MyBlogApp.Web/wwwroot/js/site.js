// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const searchInput = document.querySelector('#blog-search');
const filterButtons = [...document.querySelectorAll('[data-blog-filter]')];
const blogCards = [...document.querySelectorAll('#blog-grid [data-blog-card]')];
const emptyState = document.querySelector('#blog-empty');
let activeFilter = 'all';

function filterStories() {
    const query = searchInput?.value.trim().toLowerCase() ?? '';
    let visibleCount = 0;

    blogCards.forEach(card => {
        const searchableText = `${card.dataset.title} ${card.dataset.description} ${card.dataset.tags}`.toLowerCase();
        const matchesSearch = !query || searchableText.includes(query);
        const matchesFilter = activeFilter === 'all' || (card.dataset.tags ?? '').toLowerCase().includes(activeFilter);
        const isVisible = matchesSearch && matchesFilter;
        card.classList.toggle('d-none', !isVisible);
        if (isVisible) visibleCount += 1;
    });

    emptyState?.classList.toggle('d-none', visibleCount !== 0);
}

searchInput?.addEventListener('input', filterStories);
filterButtons.forEach(button => button.addEventListener('click', () => {
    activeFilter = button.dataset.blogFilter ?? 'all';
    filterButtons.forEach(item => item.classList.toggle('active', item === button));
    filterStories();
}));

document.querySelectorAll('[data-copy-link]').forEach(button => {
    const defaultText = button.textContent;

    button.addEventListener('click', async () => {
        try {
            await navigator.clipboard.writeText(window.location.href);
            button.textContent = 'Copied';
            button.classList.add('copied');
            setTimeout(() => {
                button.textContent = defaultText;
                button.classList.remove('copied');
            }, 1800);
        } catch {
            button.textContent = 'Copy failed';
            setTimeout(() => {
                button.textContent = defaultText;
            }, 1800);
        }
    });
});
