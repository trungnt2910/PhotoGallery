import "photoswipe/dist/photoswipe.css";
import "photoswipe/dist/default-skin/default-skin.css";
import { PhotoSwipeService } from "./PhotoSwipeService";
import { PhotoSwipeGallery } from "./PhotoSwipeGallery";

function Main()
{
    console.log("PhotoSwipe.Blazor loaded.");
}

Main();

export { PhotoSwipeService, PhotoSwipeGallery };