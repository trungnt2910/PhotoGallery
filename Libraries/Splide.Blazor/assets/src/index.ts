import "@splidejs/splide/dist/css/splide.min.css";
import { SplideView } from "./SplideView";

function Main()
{
    console.log("Splide.Blazor loaded.");
}

Main();

class Blazor
{
    static get SplideView()
    {
        return SplideView;
    }
}

// Simulate the namespace experience: The whole name will become
// Splide.Blazor.*
export { Blazor }