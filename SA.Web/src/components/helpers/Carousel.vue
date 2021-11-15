<template>
    <div class="carousel-wrapper" v-if="data && data.items.length > 0">
        <v-layout row wrap>
            <v-flex xs12 md6 class="text-md-left text-xs-center">
                <h1>{{ resx('auctions') }} {{ data.name }}</h1>
                <v-btn color="black">
                    {{ data.items.length }}
                    <span class="car-text">{{ btnCarsText(data.items.length) }}</span>
                </v-btn>
            </v-flex>
            <v-flex xs12 md6 class="text-md-right text-xs-center">
                <h3>{{ resx('from').toLowerCase() }} {{ data.validFrom | moment('DD.MM.YYYY') }} {{ resx('to').toLowerCase() }} {{ data.validTo | moment('DD.MM.YYYY') }}</h3>
            </v-flex>
        </v-layout>
        <v-layout row wrap fill-height>
            <v-flex v-if="!isMobile" class="chevron">
                <i class="material-icons" @click="moveBack">
                    chevron_left
                </i>
            </v-flex>
            <v-flex class="slider" ref="carouseSlider">
                <v-layout v-if="isMobile" row class="wrap">
                    <v-flex class="item" md2 v-for="(item, index) in showItems()" :key="index">
                        <carousel-item :item="item" @detailCarousel="detail($event)" />
                    </v-flex>
                </v-layout>
                <v-layout v-if="!isMobile" row class="overflow-hidden">
                    <v-flex class="item" md2 v-for="(item, index) in showItems()" :key="index">
                        <carousel-item :item="item" @detailCarousel="detail($event)" />
                    </v-flex>
                </v-layout>
            </v-flex>
            <v-flex v-if="!isMobile" class="chevron">
                <i class="material-icons" @click="moveForward">
                    chevron_right
                </i>
            </v-flex>
        </v-layout>
    </div>
</template>

<script lang="ts">
    import { Component, Prop, Watch } from 'vue-property-decorator';
    import { Getter, namespace } from 'vuex-class';

    import BaseComponent from '../BaseComponent.vue';
    import CarouselItem from './CarouselItem.vue';

    import { CarouselDto, CarouselItemDto } from './../../poco';

    const SettingsGetter = namespace('settings', Getter);

    @Component({
        components: {
            CarouselItem,
        },
    })
    export default class Carousel extends BaseComponent {
        @Prop({ default: undefined })
        private name: string;
        @Prop({ default: undefined })
        private data: CarouselDto;

        @SettingsGetter('getIsMobile')
        private isMobile: boolean;

        private sliderWidth: number = 0;
        private step: number = 0;
        private take: number = 1;

        @Watch('sliderWidth')
        private watchSliderWidth(sliderWidth: number) {
            if (sliderWidth !== this.sliderWidth) {
                this.sliderWidth = sliderWidth;
                this.setTake();
            }
        }

        private mounted() {
            this.sliderWidth = 0;
            this.setTake();
        }

        private moveBack(): void {
            if (this.isValidBackStep()) {
                this.step -= this.take;
            }
        }

        private moveForward(): void {
            if (this.isValidForwardStep()) {
                this.step += this.take;
            }
        }

        private showItems(): CarouselItemDto[] {
            return this.data.items.slice(this.step, this.step + this.take);
        }

        private setTake(): void {
            if (!this.isMobile) {
                this.take = 6;
            } else {
                this.take = this.data.items.length;
            }
        }

        private detail(id: number): void {
            window.location.href = `auctionDetail?id=${id}`;
        }

        private isValidForwardStep(): boolean {
            return this.step + this.take < this.data.items.length;
        }

        private isValidBackStep(): boolean {
            return this.step >= this.take;
        }

        private btnCarsText(count: number): string {
            switch (count) {
                case 1:
                    return `${this.settings.resource.car.toLowerCase()}`;
                case 2:
                case 3:
                    return `${this.settings.resource.fewCars.toLowerCase()}`;
                default:
                    return `${this.settings.resource.cars.toLowerCase()}`;
            }
        }
    }
</script>

<style>
    .carousel-wrapper {
        position: relative;
        padding-top: 30px;
        padding-bottom: 30px;
    }

        .carousel-wrapper .chevron {
            width: 30px !important;
            max-width: 30px !important;
            position: relative;
        }

        .carousel-wrapper .slider {
            width: 70%;
        }

        .carousel-wrapper .chevron .material-icons:hover {
            background-color: silver;
        }

        .carousel-wrapper .slider .item {
            padding-left: 5px !important;
            padding-right: 5px !important;
            width: 300px !important;
        }

    .slider .v-card .v-card__media__content {
        width: 300px !important;
        height: 279px !important;
        cursor: pointer !important;
    }

    .carousel-wrapper .chevron .material-icons {
        height: 80px;
        line-height: 80px;
        background-color: black;
        color: white;
        border-radius: 5px;
        cursor: pointer;
        position: absolute;
        top: 50%;
        transform: translateY(-50%);
    }

    .carousel-wrapper .slide-container .item {
        display: inline;
    }

    /* .carousel-wrapper .item h4 {
      font-family: Roboto;
      font-size: 20px;
      font-weight: 500 !important;
      font-style: normal;
      font-stretch: normal;
      line-height: 2.12;
      letter-spacing: 0px;
      color: #000000;
      white-space: normal !important;
    } */

    /* .carousel-wrapper .item .item-price h4 div {
      line-height: 1.2 !important;
    } */

    /* .carousel-wrapper .item .item-info {
      font-family: Roboto;
      font-size: 10px !important;
      font-style: normal;
      font-stretch: normal;
      letter-spacing: 0.8px;
      color: #929292 !important;
    } */

    /* .carousel-wrapper .item .item-price-text {
      text-transform: uppercase !important;
      font-family: Roboto;
      font-size: 10px !important;
      font-weight: bold;
      font-style: normal;
      font-stretch: normal;
      letter-spacing: 0.8px;
      color: #030303 !important;
    }

    .carousel-wrapper .item .price-with-dph {
      padding-left: 2px;
      text-transform: uppercase;
      font-weight: bold;
      font-size: 10px;
    } */

    .carousel-wrapper h1,
    .carousel-wrapper .v-btn {
        display: inline !important;
    }

    .carousel-wrapper .v-btn {
        padding-left: 15px !important;
        height: 23px !important;
        min-width: 50px !important;
        color: #fff !important;
        border-radius: 5px !important;
        text-transform: lowercase;
        margin-bottom: 20px;
    }

        .carousel-wrapper .v-btn span {
            padding-left: 5px;
            font-size: 10px !important;
            letter-spacing: 0.8px !important;
            color: #929292 !important;
        }

    .carousel-wrapper h1 {
        font-family: Roboto;
        font-size: 30px;
        font-weight: 500;
        font-style: normal;
        font-stretch: normal;
        letter-spacing: 0px;
        color: #000000;
        padding-top: 0px !important;
        padding-bottom: 0px !important;
    }
</style>
