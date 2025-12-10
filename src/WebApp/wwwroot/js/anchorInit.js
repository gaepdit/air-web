if (typeof AnchorJS === 'function') {
    const anchors = new AnchorJS();
    // DOMContentLoaded was tested to be the best place to call anchors.add()
    document.addEventListener('DOMContentLoaded', function () {
        anchors.options.placement = "left";
        anchors.add('h2:not(.no-anchor), h3:not(.no-anchor)');
    })
}
