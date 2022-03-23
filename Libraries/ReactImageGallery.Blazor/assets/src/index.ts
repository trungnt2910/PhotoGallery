import { ReactImageGalleryView } from "./ReactImageGalleryView"
import "react-image-gallery/styles/css/image-gallery.css";

function Main()
{
    console.log("ReactImageGallery.Blazor loaded.");
}

Main();

class Blazor
{
    static get ReactImageGalleryView()
    {
        return ReactImageGalleryView;
    }
}

// Simulate the namespace experience: The whole name will become
// Splide.Blazor.*
export { Blazor }