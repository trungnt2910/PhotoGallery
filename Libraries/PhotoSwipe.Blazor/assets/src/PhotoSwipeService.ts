import PhotoSwipe from "photoswipe";
import PhotoSwipeUI_Default from "photoswipe/dist/photoswipe-ui-default"

export class PhotoSwipeService
{
    static async OpenAsync(
        items: PhotoSwipe.Item[],
        options: PhotoSwipe.Options,
        uiConstructor: string)
    {
        var elemList = document.querySelectorAll(".pswp");
        if (elemList.length == 0)
        {
            return;
        }
        var elem = elemList[elemList.length - 1] as HTMLElement;
        if (options.getThumbBoundsFn)
        {
            var oldFunc = options.getThumbBoundsFn;
            options.getThumbBoundsFn = function(index)
            {
                var { x, y, w } = oldFunc(index);
                var { display, position } = elem.style;
                // Emulate maximized state
                elem.style.display = "block";
                elem.style.position = "fixed";
                var scrollWrap = elem.querySelector(".pswp__scroll-wrap");
                var { left, top } = scrollWrap.getBoundingClientRect();
                // Restore previous state
                elem.style.display = display;
                elem.style.position = position;
                return { x: x - left, y: y - top, w: w };
            }
        }
        var ctor = (uiConstructor) ? eval(uiConstructor): PhotoSwipeUI_Default
        var gallery = new PhotoSwipe(elem, ctor, items, options);
        gallery.init();
    }
}