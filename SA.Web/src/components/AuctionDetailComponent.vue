<template>
    <div class="detail">
        <loading-component :open="isLoading" />
        <v-container grid-list-xs pa-0 v-if="record" class="auction-detail" :id="recordIdString">
            <v-layout row wrap>
                <v-flex xs12 md6>
                    <v-carousel hide-delimiters :cycle="false">
                        <v-carousel-item
                            v-for="(item,i) in record.files"
                            :key="i"
                            :src="filePath(item)"
                        >
                        <v-layout row wrap v-if="currentUser.isFeePayed && currentUser.isAuthenticated && isCurrentUserBidding()">
                            <v-flex xs12 class="text-xs-right">
                                <v-tooltip top v-if="currentUser.userId === winnerId">
                                    <v-btn icon slot="activator" color="white">
                                        <v-icon small color="green" style="cursor: pointer">thumb_up</v-icon>
                                    </v-btn>
                                    <span>{{ resx('winning') }} </span>
                                </v-tooltip>
                                <v-tooltip top v-else>
                                    <v-btn icon slot="activator" color="white">
                                        <v-icon small color="red" style="cursor: pointer">thumb_down</v-icon>
                                    </v-btn>
                                    <span>{{ resx('notWinning') }} </span>
                                </v-tooltip>
                            </v-flex>
                        </v-layout>
                        </v-carousel-item>
                    </v-carousel>
                </v-flex>
                <v-flex xs12 md6>
                    <div class="right-side">
                        <v-layout row wrap class="item-header">
                            <v-flex xs12 md6>
                                <h1 class="hidden-sm-and-down">{{ record.name }}</h1>
                                <h2 class="hidden-md-and-up">{{ record.name }}</h2>
                            </v-flex>
                            <v-flex xs12 md6>
                                <v-layout row wrap class="header-info">
                                    <v-flex xs4>{{ resx('buildDate') }} {{ record.dateOfFirstRegistration | moment('YYYY') }}</v-flex>
                                    <v-flex xs4>{{ record.fuel }}</v-flex>
                                    <v-flex xs4>{{ record.state }}</v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                        <v-layout row wrap class="header-info2">
                            <v-flex xs6 class="info-text">{{ resx('toTheEndOfAuction') }}</v-flex>
                            <v-flex xs6 class="info-text text-xs-right">
                                {{ resx('actualPrice') }}
                                <span class="price-with-dph" v-if="record.withVat">{{ resx('withVat') }}</span>
                                <span class="price-with-dph" v-else>{{ resx('withoutVat') }}</span>
                            </v-flex>
                        </v-layout>
                        <v-layout row wrap class="header-info2">
                            <v-flex xs6 class="info2">
                                <countdown-component
                                    :id="recordIdToString(record)"
                                    :date="dateToString(record.validTo)"
                                    :startDate="dateToString(record.validFrom)" />
                            </v-flex>
                            <v-flex xs6 class="text-xs-right info2">
                                <price-component :price="record.currentPrice" />
                            </v-flex>
                        </v-layout>
                        <v-layout row wrap class="info3">
                            <v-flex xs12 class="text-xs-center">
                                <bid-component :bid="minimumBid" v-if="currentUser.isFeePayed && canBid && record.isActive" />
                            </v-flex>
                        </v-layout>
                    </div>
                </v-flex>
            </v-layout>
            <v-layout row wrap>
                <v-flex xs12 md6 class="left-side">
                    <v-container  grid-list-xs pa-0>
                        <v-layout row wrap>
                            <v-flex xs12>
                                <v-expansion-panel expand>
                                    <v-expansion-panel-content :value="expander1">
                                        <div slot="header">
                                            <h3>{{ resx('auctionDetailInformation') }}</h3>
                                        </div>
                                        <v-layout row wrap>
                                            <v-flex xs12 md6>
                                                <v-layout row wrap>
                                                    <v-flex xs6 class="info-text">{{ resx('startingPrice') }}</v-flex>
                                                    <v-flex xs6 class="info-value"><price-component :price="record.startingPrice" /></v-flex>
                                                </v-layout>
                                            </v-flex>
                                            <v-flex xs12 md6>
                                                <v-layout row wrap>
                                                    <v-flex xs6 class="info-text">{{ resx('beginningOfTheAuction') }}</v-flex>
                                                    <v-flex xs6 class="info-value">{{ record.validFrom | moment('DD.MM.YYYY HH:mm') }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                        </v-layout>
                                        <v-layout row wrap>
                                            <v-flex xs12 md6>
                                                <v-layout row wrap>
                                                    <v-flex xs6 class="info-text">{{ resx('minimumBid') }}</v-flex>
                                                    <v-flex xs6 class="info-value"><price-component :price="record.minimumBid" /></v-flex>
                                                </v-layout>
                                            </v-flex>
                                            <v-flex xs12 md6>
                                                <v-layout row wrap>
                                                    <v-flex xs6 class="info-text">{{ resx('endOfAuction') }}</v-flex>
                                                    <v-flex xs6 class="info-value">{{ record.validTo | moment('DD.MM.YYYY HH:mm') }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                        </v-layout>
                                        <v-layout row wrap>
                                            <v-flex xs12 md6>
                                                <v-layout row wrap>
                                                    <v-flex xs6 class="info-text">{{ resx('numberOfBids') }}</v-flex>
                                                    <v-flex xs6 class="info-value">{{ record.numberOfBids }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                            <v-flex xs12 md6>
                                                <v-layout row wrap>
                                                    <v-flex xs6 class="info-text">{{ resx('contactToAppointment') }}</v-flex>
                                                    <v-flex xs6 class="info-value">{{ record.contactToAppointment }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                        </v-layout>
                                    </v-expansion-panel-content>
                                </v-expansion-panel>
                                <v-expansion-panel expand>
                                    <v-expansion-panel-content :value="expander">
                                        <div slot="header">
                                            <h3>{{ resx('specification') }}</h3>
                                        </div>
                                        <v-layout row wrap>
                                            <v-flex xs12>
                                                <v-layout row wrap>
                                                    <v-flex xs8 class="info-text">{{ resx('dimensions') }}</v-flex>
                                                    <v-flex xs4 class="info-value text-xs-right">{{ record.dimensions}}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                            <v-flex xs12>
                                                <v-layout row wrap>
                                                    <v-flex xs8 class="info-text">{{ resx('maximumWeight') }}</v-flex>
                                                    <v-flex xs4 class="info-value text-xs-right">{{ record.maximumWeight }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                        </v-layout>
                                        <v-layout row wrap>
                                            <v-flex xs12>
                                                <v-layout row wrap>
                                                    <v-flex xs8 class="info-text">{{ resx('operationWeight') }}</v-flex>
                                                    <v-flex xs4 class="info-value text-xs-right">{{ record.operationWeight}}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                            <v-flex xs12>
                                                <v-layout row wrap>
                                                    <v-flex xs8 class="info-text">{{ resx('maximumWeightOfRide') }}</v-flex>
                                                    <v-flex xs4 class="info-value text-xs-right">{{ record.maximumWeightOfRide }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                        </v-layout>
                                        <v-layout row wrap>
                                            <v-flex xs12>
                                                <v-layout row wrap>
                                                    <v-flex xs8 class="info-text">{{ resx('mostTechnicallyWeightOfRide') }}</v-flex>
                                                    <v-flex xs4 class="info-value text-xs-right">{{ record.mostTechnicallyAcceptableWeight }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                            <v-flex xs12>
                                                <v-layout row wrap>
                                                    <v-flex xs8 class="info-text">{{ resx('mostTechnicallyAcceptableWeight') }}</v-flex>
                                                    <v-flex xs4 class="info-value text-xs-right">{{ record.mostTechnicallyAcceptableWeight }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                        </v-layout>
                                    </v-expansion-panel-content>
                                </v-expansion-panel>
                            </v-flex>
                        </v-layout>
                    </v-container>
                </v-flex>
                <v-flex xs12 md6 class="right-side">
                    <v-container  grid-list-xs pa-0>
                        <v-layout column fill-height>
                            <v-flex xs12 >
                                <v-expansion-panel expand>
                                    <v-expansion-panel-content :value="expander">
                                        <div slot="header">
                                            <h3>{{ resx('carInformation') }}</h3>
                                        </div>
                                        <v-layout row wrap>
                                            <v-flex xs12 md6>
                                                <v-layout row wrap>
                                                    <v-flex xs6 class="info-text">{{ resx('doors') }}</v-flex>
                                                    <v-flex xs6 class="info-value">{{ record.doors}}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                            <v-flex xs12 md6>
                                                <v-layout row wrap>
                                                    <v-flex xs6 class="info-text">{{ resx('mileAge') }}</v-flex>
                                                    <v-flex xs6 class="info-value">{{ record.mileage }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                        </v-layout>
                                        <v-layout row wrap>
                                            <v-flex xs12 md6>
                                                <v-layout row wrap>
                                                    <v-flex xs6 class="info-text">{{ resx('power') }}</v-flex>
                                                    <v-flex xs6 class="info-value">{{ record.power }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                            <v-flex xs12 md6>
                                                <v-layout row wrap>
                                                    <v-flex xs6 class="info-text">{{ resx('color') }}</v-flex>
                                                    <v-flex xs6 class="info-value">{{ record.color }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                        </v-layout>
                                        <v-layout row wrap>
                                            <v-flex xs12 md6>
                                                <v-layout row wrap>
                                                    <v-flex xs6 class="info-text">{{ resx('transmission') }}</v-flex>
                                                    <v-flex xs6 class="info-value">{{ record.transmission }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                            <v-flex xs12 md6>
                                                <v-layout row wrap>
                                                    <v-flex xs6 class="info-text">{{ resx('numberOfSeets') }}</v-flex>
                                                    <v-flex xs6 class="info-value">{{ record.numberOfSeets }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                        </v-layout>
                                        <v-layout row wrap>
                                            <v-flex xs12 md6>
                                                <v-layout row wrap>
                                                    <v-flex xs6 class="info-text">{{ resx('axle') }}</v-flex>
                                                    <v-flex xs6 class="info-value">{{ record.axle }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                            <v-flex xs12 md6>
                                                <v-layout row wrap>
                                                    <v-flex xs6 class="info-text">{{ resx('euroNorm') }}</v-flex>
                                                    <v-flex xs6 class="info-value">{{ record.euroNorm }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                        </v-layout>
                                        <v-layout row wrap>
                                        <v-flex xs12 md6>
                                                <v-layout row wrap>
                                                    <v-flex xs6 class="info-text">{{ resx('technicalViewOfTheVehicle') }}</v-flex>
                                                    <v-flex xs6 class="info-value">{{ record.stk | moment('DD.MM.YYYY') }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                            <v-flex xs12 md6>
                                                <v-layout row wrap>
                                                    <v-flex xs5 class="info-text">{{ resx('vehicleVinNumber') }}</v-flex>
                                                    <v-flex xs7 class="info-value">{{ record.vin }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                        </v-layout>
                                        <v-layout row wrap>
                                            <v-flex xs12>
                                                <v-layout row wrap>
                                                    <v-flex xs6 class="info-text">{{ resx('auditControlIsProvidedBy') }}</v-flex>
                                                    <v-flex xs6 class="info-value">{{ record.registrationCheck  }}</v-flex>
                                                </v-layout>
                                            </v-flex>
                                        </v-layout>
                                    </v-expansion-panel-content>
                                </v-expansion-panel>
                                <v-expansion-panel expand>
                                    <v-expansion-panel-content :value="expander">
                                        <div slot="header">
                                            <h3>{{ resx('equipment') }}</h3>
                                        </div>
                                        <v-layout row wrap>
                                            <v-flex xs12 class="info-value text-xs-left extra-padding" v-html="record.equipment">
                                            </v-flex>
                                        </v-layout>
                                    </v-expansion-panel-content>
                                </v-expansion-panel>
                                <v-expansion-panel expand>
                                    <v-expansion-panel-content :value="expander">
                                        <div slot="header">
                                            <h3>{{ resx('defects') }}</h3>
                                        </div>
                                        <v-layout row wrap>
                                            <v-flex xs12 class="info-value text-xs-left extra-padding" v-html="record.defects">
                                            </v-flex>
                                        </v-layout>
                                    </v-expansion-panel-content>
                                </v-expansion-panel>
                                <v-expansion-panel expand>
                                    <v-expansion-panel-content :value="expander">
                                        <div slot="header">
                                            <h3>{{ resx('moreDescription') }}</h3>
                                        </div>
                                        <v-layout row wrap>
                                            <v-flex xs12 class="info-value text-xs-left extra-padding" v-html="record.moreDescription">
                                            </v-flex>
                                        </v-layout>
                                    </v-expansion-panel-content>
                                </v-expansion-panel>
                            </v-flex>
                        </v-layout>
                    </v-container>
                </v-flex>
            </v-layout>
            <v-layout row wrap>
                <v-flex xs12>
                    <carousel :data="auctions[0]" @detail="loadDetail($event)" />
                </v-flex>
            </v-layout>
        </v-container>
    </div>
</template>

<script lang="ts">

import { Component, Prop, Watch } from 'vue-property-decorator';
import { Getter, Action, namespace } from 'vuex-class';
import { clearTimeout, setTimeout } from 'timers';

import BaseComponent from './BaseComponent.vue';

import CountdownComponent from './helpers/CountdownComponent.vue';
import PriceComponent from './helpers/PriceComponent.vue';
import LoadingComponent from './helpers/LoadingComponent.vue';
import Carousel from './helpers/Carousel.vue';
import BidComponent from './helpers/BidComponent.vue';

import {
    Record,
    Bid,
    User,
} from './../model';

import {
    FileSimpleDto,
    AuthUser,
    RecordTableDto,
    CarouselDto,
} from './../poco';

const AuthGetter = namespace('auth', Getter);
const RecordGetter = namespace('record', Getter);
const RecordAction = namespace('record', Action);
const AuctionGetter = namespace('auction', Getter);
const AuctionAction = namespace('auction', Action);

@Component({
    components: {
        PriceComponent,
        BidComponent,
        CountdownComponent,
        Carousel,
        LoadingComponent,
    },
})
export default class AuctionDetalComponent extends BaseComponent {
    @AuthGetter('getCurrentLoggedUser')
    private currentUser: AuthUser;

    @RecordGetter('getCurrent')
    private record: Record;

    @RecordGetter('getCurrentWinnerId')
    private winnerId: number;

    @RecordAction('getDetail')
    private detail: any;
    @RecordAction('loadAllPublished')
    private loadRecods: any;
    @RecordAction('getRecordsLastBid')
    private updateWinnerId: any;

    @AuctionGetter('getAuctionsCarousel')
    private auctions: CarouselDto[];

    @AuctionAction('getFutureAutions')
    private featuredAcutions: any;

    private isLoading: boolean = false;
    private expander: boolean[] = [true, true, true, true, true, true];
    private expander1: boolean[] = [true];
    private canBid: boolean = false;

    private winnigRefreshCounter: any = null;
    private checkEndAucitonCoutner: any = null;

    @Watch('record.bids')
    private watchBids(newBids) {
        if (this.winnigRefreshCounter == null) {
            clearInterval(this.winnigRefreshCounter);
            this.startWinningRefreshCounter();
        }
    }

    private mounted() {
        if (this.record.id !== undefined) {
            this.detail(this.$route.query.id).then((result) => {
                clearInterval(this.checkEndAucitonCoutner);
                this.startCheckEndAuction();
            });
        } else {
            clearInterval(this.checkEndAucitonCoutner);
            this.startCheckEndAuction();
        }

        this.featuredAcutions();

        clearInterval(this.winnigRefreshCounter);
        this.startWinningRefreshCounter();
    }

    private startWinningRefreshCounter(): void {
        this.winnigRefreshCounter = setInterval(() => {
            this.updateWinnerId(this.record.id);
        }, 30000); // every 30s
    }

    private startCheckEndAuction(): void {
        this.checkEndAucitonCoutner = setInterval(() => {
            if (!this.canBidFnc(this.record.validFrom, this.record.validTo)) {
                clearInterval(this.checkEndAucitonCoutner);
            }
        }, 1000);
    }

    private beforeDestroy() {
        if (this.winnigRefreshCounter !== null) {
            clearInterval(this.winnigRefreshCounter);
        }

        if (this.checkEndAucitonCoutner !== null) {
            clearInterval(this.checkEndAucitonCoutner);
        }
    }

    private updated() {
        window.scrollTo(0, 0);
    }

    private recordIdToString(record: RecordTableDto): string {
        if (record.id !== undefined) {
            return record.id.toString();
        }
    }

    private filePath(file: FileSimpleDto): string {
        return `${this.settings.apiUrl.replace('/api', '')}/${file.path}/${file.recordId}/images/${file.name}`;
    }

    private sellerInfo(user: User): string {
        return `${user.customer.companyName}`;
    }

    private dateToString(date: Date): string {
        if (date !== undefined) {
            return date.toString();
        }
    }

    private isCurrentUserBidding(): boolean {
        const biddingIds =  this.record.bids.map((x) => x.userId);

        if (biddingIds.length <= 0) {
            return false;
        }

        return biddingIds.indexOf(this.currentUser.userId) !== -1;
    }

    private canBidFnc(validFrom: Date, validTo: Date): boolean {
        const canBid =  new Date(validFrom) <= new Date()
            && new Date(validTo) >= new Date();

        this.canBid = canBid;

        return canBid;
    }

    private loadDetail(id: number): void {
        this.isLoading = true;
        this.detail(id).then((response) => {
            const result = response as boolean;
            this.isLoading = false;
            if (result) {
                this.$router.push({ path: `/auctionDetail?id=${id}` });
            }
        });
    }

    get minimumBid(): number {
        const bid = this.record.bids.length === 0
            ? this.record.currentPrice
            : this.record.currentPrice + this.record.minimumBid;
        return bid;
    }

    get recordIdString(): string {
        if (this.record.id !== undefined) {
            return `record-${this.record.id}`;
        }
    }
}

</script>

<style>

.auction-detail .header-info2 {
    background-color: black !important;
    max-height: 100px !important;
    color: white !important;
}

.auction-detail .info3 {
    background-color: #ededed !important;
    border-bottom-left-radius: 5px !important;
    border-bottom-right-radius: 5px !important;
    margin: 0 auto !important;
}

.auction-detail .bid-component {
    padding-top: 20px !important;
    max-width: 80% !important;
    margin: auto !important;
    position: relative !important;
}

.auction-detail .hidden-md-and-up {
    padding-left: 15px !important;
    padding-right: 15px !important;
}

.auction-detail .header-info2 .info2 {
  font-size: 22px !important;
  font-weight: 500;
  font-style: normal;
  font-stretch: normal;
  letter-spacing: 0px;
  text-align: left;
  padding-right: 15px !important;
  padding-left: 15px !important;
}

.auction-detail .item-header {
    padding-top: 30px !important;
}

.auction-detail .header-info .flex {
  line-height: 5 !important;
  text-align: center !important;
  font-size: 16px;
  font-weight: normal;
  font-style: normal;
  font-stretch: normal;
  line-height: normal;
  letter-spacing: 0.9px;
  text-align: left;
  color: #929292;
}

.auction-detail h1 {
  font-size: 40px !important;
  font-weight: 500;
  font-style: normal;
  font-stretch: normal;
  line-height: 1.33;
  letter-spacing: 0px;
  text-align: left;
  color: #000000;
}

.auction-detail h2 {
  font-size: 25px !important;
  font-weight: 500;
  font-style: normal;
  font-stretch: normal;
  line-height: 1.33;
  letter-spacing: 0px;
  text-align: left;
  color: #000000;
}

.auction-detail .info-text {
  font-size: 10px !important;
  font-weight: bold;
  font-style: normal;
  font-stretch: normal;
  line-height: 3;
  letter-spacing: 0.8px;
  text-align: left;
  color: #929292;
  padding-left: 15px !important;
  text-transform: uppercase;
  padding-right: 15px !important;
}

.auction-detail {
    font-family: Roboto !important;
    padding-bottom: 20px !important;
}

.auction-detail .info-value {
  font-size: 13px !important;
  font-weight: bold;
  font-style: normal;
  font-stretch: normal;
  line-height: 2;
  letter-spacing: 0.8px;
  text-align: right;
  padding-right: 15px !important;
  color: #000000;
}

.auction-detail .extra-padding {
    padding-left: 15px !important;
}

.auction-detail .v-carousel {
    --webkit-box-shadow: 0 0px 0px 0px rgba(0,0,0,0), 0 0px 0px 0 rgba(0,0,0,0), 0 0px 0px 0 rgba(0,0,0,0) !important;
    box-shadow: 0 0px 0px 0px rgba(0,0,0,0), 0 0px 0px 0 rgba(0,0,0,0), 0 0px 0px 0 rgba(0,0,0,0) !important;
    max-width: 600px;
    max-height: 430px;
    border-radius: 5px !important;
    border: 1px solid #e6e6e6 !important;
}

.auction-detail .v-expansion-panel {
    border-radius: 5px !important;
    margin-top: 25px !important;
    --webkit-box-shadow: 0 0px 0px 0px rgba(0,0,0,0), 0 0px 0px 0 rgba(0,0,0,0), 0 0px 0px 0 rgba(0,0,0,0) !important;
    box-shadow: 0 0px 0px 0px rgba(0,0,0,0), 0 0px 0px 0 rgba(0,0,0,0), 0 0px 0px 0 rgba(0,0,0,0) !important;
    border: 1px solid #e6e6e6 !important;
}

.auction-detail .v-expansion-panel__header {
    background-color: #ededed !important;
}

.auction-detail .v-expansion-panel__container {
    border: 1px solid #e6e6e6 !important;
}

.auction-detail .v-expansion-panel__header h3 {
    font-size: 15px !important;
    font-weight: bold !important;
    font-style: normal;
    font-stretch: normal;
    line-height: 3.87;
    letter-spacing: 1.2px;
    text-align: left;
    color: #000000;
    text-transform: uppercase;
}

.auction-detail .v-expansion-panel__body {
    background-color: #ffffff !important;
}

.header-info2 .price-with-dph {
  padding-left: 2px;
  text-transform: uppercase;
  font-weight: bold;
  font-size: 10px;
}

.auction-detail .count-down {
    font-size: 22px !important;
}

</style>