/// <reference path="jquery.d.ts" />
export declare class GalleryConfig {
    public galleryDom: JQuery;
    public imageDom: JQuery;
    public getImageUrl: (imageData: ImageData, x: number, y: number) => string;
    public focusImage: (gallery: GalleryData, image: ImageData) => void;
    public getImageSize: (x: number, y: number) => ImageSize;
    public cacheSize: number;
}
export declare class ImageData {
    public id: string;
}
export declare class GalleryData {
    public images: ImageData[];
}
export declare class ImageSize {
    public x: number;
    public y: number;
}
export declare class Gallery {
    public currentImageId: string;
    public galleryConfig: GalleryConfig;
    public galleryData: GalleryData;
    public currentImage: ImageData;
    public currentImagePosition: number;
    public imageDomNow: JQuery;
    public imageDomFadeout: JQuery;
    public imagesPreload: HTMLImageElement[];
    public imageSize: ImageSize;
    public slideshowIntervalId: number;
    constructor(galleryConfig: GalleryConfig);
    public show(galleryData: GalleryData, imageId: string): void;
    public close(): void;
    public gotoNextImage(): void;
    public gotoPreviousImage(): void;
    public focusImage(imageId: string): void;
    public startSlideshow(): void;
    public stopSlideshow(): void;
    public getActiveImage(): ImageData;
    public __getPositionOfImage(imageId: string): number;
    public __moveToImage(imagePosition: number): void;
    public __getUrlOfImage(image: ImageData): string;
    public __slideshowEvent(tthis: Gallery): void;
    public __fillCache(imagePosition: number): void;
}
