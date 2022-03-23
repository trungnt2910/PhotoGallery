import { Item } from "photoswipe";
import { PhotoSwipeService } from "./PhotoSwipeService";

export class PhotoSwipeGallery
{
    private _elem: HTMLElement;
    private _items: Item[];
    private _imgs: HTMLImageElement[];
    private _photoSwipeUIConstructor: string;

    private static Closest(el: Element, fn: (e: Element) => boolean) : Element
    {
        return el && (fn(el) ? el : PhotoSwipeGallery.Closest(el.parentNode as Element, fn));
    };

    private OnThumbnailsClick(e: Event)
    {
        e = e || window.event;
        e.preventDefault ? e.preventDefault() : e.returnValue = false;

        var eTarget = e.target || e.srcElement;

        // find root element of slide
        var clickedListItem = PhotoSwipeGallery.Closest(eTarget as Element, function (el: Element)
        {
            return (el.tagName && el.tagName.toUpperCase() === 'FIGURE');
        });

        if (!clickedListItem)
        {
            return;
        }

        // find index of clicked item by looping through all child nodes
        // alternatively, you may define index via data- attribute
        var clickedGallery = clickedListItem.parentNode,
            childNodes = clickedListItem.parentNode.childNodes,
            numChildNodes = childNodes.length,
            nodeIndex = 0,
            index;

        for (var i = 0; i < numChildNodes; i++)
        {
            if (childNodes[i].nodeType !== 1)
            {
                continue;
            }

            if (childNodes[i] === clickedListItem)
            {
                index = nodeIndex;
                break;
            }
            nodeIndex++;
        }

        if (index >= 0)
        {
            // open PhotoSwipe if valid index found
            this.OpenPhotoSwipe(index);
        }
        return false;
    }

    private async OpenPhotoSwipe(index: number)
    {
        var imgs = this._imgs;
        await PhotoSwipeService.OpenAsync(this._items,
            {
                index: index,
                getThumbBoundsFn: function (index) {
                    // See Options -> getThumbBoundsFn section of documentation for more info
                    var thumbnail = imgs[index], // find thumbnail
                        pageYScroll = window.pageYOffset || document.documentElement.scrollTop,
                        rect = thumbnail.getBoundingClientRect();

                    return { x: rect.left, y: rect.top + pageYScroll, w: rect.width };
                }
            }, this._photoSwipeUIConstructor);
    }

    constructor(elem: HTMLElement, items: Item[], uiConstructor: string)
    {
        this._elem = elem;
        this._items = items;
        this._imgs = Array.from(this._elem.querySelectorAll("img"));
        this._photoSwipeUIConstructor = uiConstructor;

        var instance = this;
        this._elem.onclick = function (e) { instance.OnThumbnailsClick(e) };
    }

    static Construct(elem: HTMLElement, items: Item[], uiConstructor: string)
    {
        return new PhotoSwipeGallery(elem, items, uiConstructor);
    }
}