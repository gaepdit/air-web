function createTOC(headerTags) {
    const toc = document.getElementById('toc');

    const tags = headerTags.join(', ');
    const headers = document.querySelectorAll(tags);

    headers.forEach((header, index) => {
        // creates ID if H tag has none
        if (!header.id) {
            header.id = `section${index + 1}`;
        }

        const link = document.createElement('a');
        link.href = `#${header.id}`;
        link.textContent = header.textContent;
        link.classList.add('list-group-item', 'list-group-item-action');

        toc.appendChild(link);
    });
}
