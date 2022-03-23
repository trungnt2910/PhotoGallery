import ImageGallery, { ReactImageGalleryProps } from "react-image-gallery";
import React from "react";
import ReactDOM from "react-dom";

export class ReactImageGalleryView
{
    static Render(elem: HTMLElement, options: ReactImageGalleryProps)
    {
        var reactElem = React.createElement(ImageGallery, options);

        ReactDOM.render(reactElem, elem);
    }
}