/// <reference path="jquery.d.ts" />

export class GalleryConfig
{
	galleryDom : JQuery;
	imageDom: JQuery;
	getImageUrl: (imageData : ImageData, x : number, y : number) => string;
	focusImage: (gallery : GalleryData, image : ImageData) => void;
	getImageSize: (x : number, y : number) => ImageSize;
	cacheSize : number;
}

export class ImageData
{
	id : string;
}

export class GalleryData
{
	images : ImageData[];
}

export class ImageSize
{
	x : number;
	y : number;
}

export class Gallery {
    // Checks, if galleryconfig contains all necessary functions
    currentImageId = "";
    galleryConfig: GalleryConfig;
    galleryData: GalleryData;
    currentImage: ImageData;
    currentImagePosition: number;

    // Stores the imageDom, which contains the currently visible building
    imageDomNow: JQuery;

    // Stores the imageDom, which is currently faded out. 
    imageDomFadeout: JQuery;

    // Stores an array of imagedoms that will be used for preloading. 
    // Key of the array will be the position as in galleryData.images. 
    // Value the Image instance
    imagesPreload: HTMLImageElement[];

    imageSize: ImageSize;

    slideshowIntervalId: number;

    constructor(galleryConfig: GalleryConfig) {
        this.imagesPreload = new Array < HTMLImageElement > ();
        this.galleryConfig = galleryConfig;

        if (galleryConfig === undefined) {
            throw "galleryConfig not defined";
        }

        if (galleryConfig.galleryDom === undefined) {
            throw "galleryConfig.galleryDom === undefined";
        }

        if (galleryConfig.imageDom === undefined) {
            galleryConfig.imageDom = $(".galleryimage", galleryConfig.galleryDom);
        }

        if (galleryConfig.getImageUrl === undefined) {
            throw "galleryConfig.getImageUrl === undefined";
        }

        if (galleryConfig.focusImage === undefined) {
            galleryConfig.focusImage = function () { /*Dummy*/ };
        }

        if (galleryConfig.getImageSize === undefined) {
            throw "galleryConfig.getImageSize === undefined";
        }

        if (galleryConfig.cacheSize === undefined) {
            galleryConfig.cacheSize = 10;
        }
    }

    show(galleryData: GalleryData, imageId: string) {
        var tthis = this;
        this.galleryData = galleryData;
        // Check images
        var images = this.galleryData.images;
        if (images === undefined) {
            throw "galleryData.images === undefined";
        }

        if (images.length == 0) {
            throw "galleryData.images.length == 0";
        }

        // Determines size of gallery out of window size
        this.imageSize =
            this.galleryConfig.getImageSize(window.innerWidth, window.innerHeight);
        if (this.imageSize.x === undefined || this.imageSize.y === undefined) {
            throw "this.imageSize.x or .y is not set by this.galleryConfig.getImageSize";
        }

        // Resets the full stuff
        $(".dynamic", this.galleryConfig.imageDom).remove();

        // Creates empty image element
        var divImage = $("<div></div>");
        divImage.attr('class', 'dynamic');
        divImage.css('width', this.imageSize.x);
        divImage.css('height', this.imageSize.y);
        this.galleryConfig.imageDom.append(divImage);

        // Binds buttons
        $(".prevbutton", this.galleryConfig.galleryDom).bind('click.gallery', function () { tthis.gotoPreviousImage(); });
        $(".nextbutton", this.galleryConfig.galleryDom).bind('click.gallery', function () { tthis.gotoNextImage(); });
        $(".closebutton", this.galleryConfig.galleryDom).bind('click.gallery', function () { tthis.close(); });
        $(".startslideshowbutton", this.galleryConfig.galleryDom).bind('click.gallery', function () { tthis.startSlideshow(); });
        $(".stopslideshowbutton", this.galleryConfig.galleryDom).bind('click.gallery', function () { tthis.startSlideshow(); });

        // Check, if imageId is given and found in images
        var position = this.__getPositionOfImage(imageId);
        if (position != -1) {
            this.__moveToImage(position);
        }

        this.galleryConfig.galleryDom.show();
    }

    close() {
        this.stopSlideshow();

        $(".prevbutton", this.galleryConfig.galleryDom).unbind('click.gallery');
        $(".nextbutton", this.galleryConfig.galleryDom).unbind('click.gallery');
        $(".closebutton", this.galleryConfig.galleryDom).unbind('click.gallery');
        $(".startslideshowbutton", this.galleryConfig.galleryDom).unbind('click.gallery');
        $(".stopslideshowbutton", this.galleryConfig.galleryDom).unbind('click.gallery');

        $(".dynamic", this.galleryConfig.imageDom).empty();
        this.galleryConfig.galleryDom.hide();
    }

    gotoNextImage() {
        var nextPosition = this.currentImagePosition + 1;
        if (nextPosition >= this.galleryData.images.length) {
            nextPosition = 0;
        }

        this.__moveToImage(nextPosition);
    }

    gotoPreviousImage() {
        var nextPosition = this.currentImagePosition - 1;
        if (nextPosition < 0) {
            nextPosition = this.galleryData.images.length - 1;
        }

        this.__moveToImage(nextPosition);
    }

    focusImage(imageId: string) {
        var position = this.__getPositionOfImage(imageId);
        if (position != -1) {
            this.__moveToImage(position);
        }
    }

    startSlideshow() {
        var tthis = this;
        if (this.slideshowIntervalId === undefined) {
            var tthis = this;
            this.slideshowIntervalId = window.setInterval(
                function () {
                    tthis.__slideshowEvent(tthis);
                },
                3000);
        }
    }

    stopSlideshow() {
        if (this.slideshowIntervalId !== undefined) {
            this.slideshowIntervalId = undefined;
        }
    }

    getActiveImage() {
        return this.currentImage;
    }

    __getPositionOfImage(imageId: string) {
        for (var n = 0; n < this.galleryData.images.length; n++) {
            var image = this.galleryData.images[n];
            if (image.id === imageId) {
                return n;
            }
        }

        return -1;
    }

    __moveToImage(imagePosition: number) {
        var tthis = this;
        this.currentImagePosition = imagePosition;
        this.currentImage = this.galleryData.images[imagePosition];
        if (this.currentImage === undefined) {
            throw "Image at position " + imagePosition + " not found";
        }

        this.currentImageId = this.currentImage.id;

        // Shows the image
        if (this.imageDomNow !== undefined) {
            this.imageDomFadeout = this.imageDomNow;

            var currentImage = this.imageDomFadeout;
            this.imageDomFadeout.fadeOut(function () {
                currentImage.remove();
            });
        }

        this.imageDomNow = $("<img />");
        this.imageDomNow.attr('class', 'dynamic');
        this.imageDomNow.css('position', 'absolute');
        this.imageDomNow.css('width', this.imageSize.x);
        this.imageDomNow.css('height', this.imageSize.y);
        this.imageDomNow.attr('src', this.__getUrlOfImage(this.currentImage));
        this.galleryConfig.imageDom.prepend(this.imageDomNow);
        this.imageDomNow.hide();
        this.imageDomNow.fadeIn();

        // Now fills the cache
        this.__fillCache(imagePosition);

        // Calls
        this.galleryConfig.focusImage(this.galleryData, this.currentImage);
    }

    __getUrlOfImage(image: ImageData) {
        if (image === undefined) {
            throw "Invalid";
        }

        return this.galleryConfig.getImageUrl(image, this.imageSize.x, this.imageSize.y);
    }

    __slideshowEvent(tthis: Gallery) {
        tthis.gotoNextImage();
    }

    __fillCache(imagePosition: number) {
        var after = Math.max(1, Math.round(this.galleryConfig.cacheSize * 2 / 3));
        var before = Math.max(1, this.galleryConfig.cacheSize - after - 1);
        var first = imagePosition - before;
        var last = imagePosition + after;

        if (first < 0) {
            first = 0;
        }

        if (last >= this.galleryData.images.length) {
            last = this.galleryData.images.length - 1;
        }

        for (var n = first; n <= last; n++) {
            // Checks, if image is already loaded, otherwise load
            if (this.imagesPreload[n] === undefined) {
                var image = new Image();
                image.src = this.__getUrlOfImage(this.galleryData.images[n]);
                this.imagesPreload[n] = image;
            }
        }
    }
}