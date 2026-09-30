// Adapted from https://www.xjavascript.com/blog/javascript-html-autocomplete-textbox/
class Autocomplete {
    constructor(options) {
        // Required setup options
        this.input = options.input; // The input from the form field.
        this.data = options.data; // The JSON array to search
        this.searchProperties = options.searchProperties; // The JSON object properties to search within.
        this.displayProperties = options.displayProperties; // The JSON properties to show in the results list.
        this.fillProperty = options.fillProperty; // The JSON property to fill the input element with.

        // Optional setup options
        this.minLength = options.minLength || 3; // Default: suggest after 1 character.
        this.maxResults = options.maxResults || 10; // Default: return the first 10 results.

        // Finish setup
        this.currentFocus = -1;
        this.suggestionsList = this.input.nextElementSibling;
        this.bindEvents();
    }

    // Bind all event listeners
    bindEvents() {
        this.input.addEventListener('input', (e) => this.handleInput(e));
        this.input.addEventListener('keydown', (e) => this.handleKeydown(e));
        document.addEventListener('click', () => this.closeSuggestions());
    }

    // Handle user input (filter data and show suggestions)
    handleInput(e) {
        const inputValue = e.target.value.trim().toLowerCase();
        this.closeSuggestions();

        if (inputValue.length < this.minLength) return;

        const matches = this.data.filter(
            function (item) {
                if (this.count >= this.maxResults) return false;
                return this.searchProperties.some((property) => {
                    if (item[property].toLowerCase().includes(inputValue)) {
                        this.count++;
                        return true;
                    }
                    return false;
                });
            }, {count: 0, maxResults: this.maxResults, searchProperties: this.searchProperties});

        if (matches.length > 0) {
            this.renderSuggestions(matches);
            this.suggestionsList.style.display = 'block';
        }
    }

    // Render matching suggestions as <li> elements
    renderSuggestions(matches) {
        matches.forEach(item => {
            const li = document.createElement('li');
            li.classList.add('list-group-item');
            const textArray = [];
            this.displayProperties.forEach(prop => textArray.push(item[prop]));
            li.textContent = textArray.join(' – ');
            li.dataset.fill = item[this.fillProperty];
            li.addEventListener('click', () => this.selectItem(item[this.fillProperty]));
            this.suggestionsList.appendChild(li);
        });
    }

    // Handle selection (click or Enter)
    selectItem(item) {
        this.input.value = item;
        this.closeSuggestions();
    }

    // Handle keyboard navigation (arrow keys/Enter)
    handleKeydown(e) {
        const items = this.suggestionsList.getElementsByTagName('li');

        if (e.key === 'ArrowDown') {
            this.currentFocus++;
            this.addActive(items);
            e.preventDefault();
        } else if (e.key === 'ArrowUp') {
            this.currentFocus--;
            this.addActive(items);
            e.preventDefault();
        } else if (e.key === 'Enter') {
            if (this.suggestionsList.style.display === 'none') return;
            e.preventDefault();
            if (this.currentFocus > -1 && items[this.currentFocus]) {
                this.selectItem(items[this.currentFocus].dataset.fill);
            }
        } else if (e.key === 'Escape') {
            this.closeSuggestions();
            e.preventDefault();
        }
    }

    // Highlight active suggestion
    addActive(items) {
        if (!items.length) return;
        this.removeActive(items);
        this.currentFocus = (this.currentFocus + items.length) % items.length;
        items[this.currentFocus].classList.add('active');
    }

    // Remove active state from all suggestions
    removeActive(items) {
        Array.from(items).forEach(item => item.classList.remove('active'));
    }

    // Clear and hide suggestions list
    closeSuggestions() {
        this.suggestionsList.innerHTML = '';
        this.suggestionsList.style.display = 'none';
        this.currentFocus = -1;
    }
}
