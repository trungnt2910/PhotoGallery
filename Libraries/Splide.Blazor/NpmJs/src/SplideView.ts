import Splide, { Options } from "@splidejs/splide";

export class SplideView
{
    private readonly _splide: Splide;
    private readonly _elem: HTMLElement;

    Mount()
    {
        this._splide.mount();
    }

    constructor(elem: HTMLElement, options: Options)
    {
        this._elem = elem;
        this._splide = new Splide(this._elem, options);
    }

    static Construct(elem: HTMLElement, options: Options)
    {
        return new SplideView(elem, options);
    }
}