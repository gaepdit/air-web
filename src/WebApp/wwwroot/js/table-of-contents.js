function createTOC(headerTags) {
    const toc = document.getElementById('toc');
    const headers = document.querySelectorAll(headerTags.join(', '));

    headers.forEach((header, index) => {
        // create ID if H tag has none
        if (!header.id) header.id = `section${index + 1}`;

        const link = document.createElement('a');
        link.href = `#${header.id}`;
        link.textContent = header.textContent;
        link.classList.add('list-group-item', 'list-group-item-action', 'toc-item', 'px-1', 'py-1');

        // indent H3
        if (header.tagName === 'H3') link.classList.add('ps-3');

        toc.appendChild(link);
    });
}
